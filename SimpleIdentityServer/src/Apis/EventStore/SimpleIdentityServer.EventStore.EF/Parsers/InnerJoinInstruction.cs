﻿#region copyright
// Copyright 2017 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using SimpleIdentityServer.EventStore.EF.Extensions;
using SimpleIdentityServer.EventStore.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleIdentityServer.EventStore.EF.Parsers
{
    public class InnerJoinInstruction : BaseInstruction
    {
        private const string innerKey = "inner";
        private const string outerKey = "outer";
        private const string selectKey = "select";
        private readonly IEnumerable<string> parameters = new List<string>
        {
            innerKey,
            outerKey
        };

        private readonly IEnumerable<string> selectParameters = new List<string>
        {
            innerKey,
            outerKey
        };

        public const string Name = "join";

        public InnerJoinInstruction()
        {
        }

        public override KeyValuePair<string, Expression>? GetExpression<TSource>(Type outerSourceType, ParameterExpression rootParameter, IEnumerable<TSource> source)
        {
            if (outerSourceType == null)
            {
                throw new ArgumentNullException(nameof(outerSourceType));
            }

            // 1. Split the value & extract the field names or requests.
            var splitted = Parameter.Split(',');
            var instructions = splitted.Select(s => InstructionHelper.ExtractInstruction(s)).Where(s => s.HasValue);
            if (!parameters.Any(p => instructions.Any(i => i.Value.Key == p)))
            {
                throw new ArgumentException("either inner or outer parameter is not specified");
            }

            // 2. Construct the expressions.
            var outer = instructions.First(i => i.Value.Key == outerKey).Value.Value;
            var inner = instructions.First(i => i.Value.Key == innerKey).Value.Value;
            var select = instructions.FirstOrDefault(i => i.Value.Key == selectKey);
            var selectValue = string.Empty;
            if (select != null)
            {
                selectValue = select.Value.Value;
            }

            Type innerSourceType = outerSourceType;
            Expression targetExpression = null;
            if (TargetInstruction != null)
            {
                var targetArg = Expression.Parameter(typeof(IQueryable<TSource>), "t");
                targetExpression = TargetInstruction.GetExpression(outerSourceType, targetArg, source).Value.Value;
                innerSourceType = targetExpression.Type.GetGenericArguments().First();
            }

            Type tResult = innerSourceType;
            var fieldNames = outer.GetParameters();
            var outerArg = Expression.Parameter(outerSourceType, "x");
            var innerArg = Expression.Parameter(innerSourceType, "y");
            LambdaExpression outerLambda = null,
                innerLambda = null;
            Type resultType;
            if (fieldNames.Count() == 1)
            {
                resultType = outerSourceType.GetProperty(fieldNames.First()).PropertyType;
                var outerProperty = Expression.Property(outerArg, fieldNames.First());
                var innerProperty = Expression.Property(innerArg, fieldNames.First());
                outerLambda = Expression.Lambda(outerProperty, new ParameterExpression[] { outerArg });
                innerLambda = Expression.Lambda(innerProperty, new ParameterExpression[] { innerArg });
            }
            else
            {
                var commonAnonType = ReflectionHelper.CreateNewAnonymousType(outerSourceType, fieldNames);
                resultType = commonAnonType.AsType();
                var outerNewExpr = ExpressionBuilder.BuildNew(outer, outerSourceType, commonAnonType, outerArg);
                var innerNewExpr = ExpressionBuilder.BuildNew(inner, innerSourceType, commonAnonType, innerArg);
                outerLambda = Expression.Lambda(outerNewExpr, new ParameterExpression[] { outerArg });
                innerLambda = Expression.Lambda(innerNewExpr, new ParameterExpression[] { innerArg });
            }

            LambdaExpression selectorResult = null;
            if (string.IsNullOrWhiteSpace(selectValue))
            {
                selectorResult = Expression.Lambda(outerArg, new ParameterExpression[] { outerArg, innerArg });
            }
            else
            {
                var selectAttributes = new List<KeyValuePair<string, ICollection<string>>>();
                foreach(var val in selectValue.Split('|'))
                {
                    var values = val.Split('$');
                    if (values.Count() != 2 && values.Count() != 1)
                    {
                        continue;
                    }

                    var key = values.First();
                    var kvp = selectAttributes.FirstOrDefault(s => s.Key == key);
                    if (kvp.IsEmpty())
                    {
                        kvp = new KeyValuePair<string, ICollection<string>>(key, new List<string>());
                        selectAttributes.Add(kvp);
                    }

                    if (values.Count() == 2)
                    {
                        kvp.Value.Add(values.ElementAt(1));
                    }
                }

                if (!selectAttributes.Any(a => selectParameters.Contains(a.Key)))
                {
                    throw new InvalidOperationException("At least one parameter in select is not supported");
                }

                Dictionary<string, Type> mapping = new Dictionary<string, Type>();
                var outerSelect = selectAttributes.FirstOrDefault(a => a.Key == outerKey);
                var innerSelect = selectAttributes.FirstOrDefault(a => a.Key == innerKey);
                AddTypes(mapping, "outer", outerSourceType, outerSelect);
                AddTypes(mapping, "inner", innerSourceType, innerSelect);
                var anonymousType = ReflectionHelper.CreateNewAnonymousType(mapping);
                var parameters = new List<Expression>();
                var tmp = GetParameterExpressions(outerArg, outerSelect);
                if (tmp != null)
                {
                    parameters.AddRange(tmp);
                }

                tmp = GetParameterExpressions(innerArg, innerSelect);
                if (tmp != null)
                {
                    parameters.AddRange(tmp);
                }

                var newExpr = Expression.New(anonymousType.DeclaredConstructors.First(), parameters);
                selectorResult = Expression.Lambda(newExpr, new ParameterExpression[] { innerArg, outerArg });
                tResult = anonymousType.AsType();
            }
            
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods().Where(m => m.Name == "Join" && m.IsGenericMethodDefinition).Where(m => m.GetParameters().ToList().Count == 5).First();
            var genericMethod = method.MakeGenericMethod(outerSourceType, innerSourceType, resultType, tResult);
            MethodCallExpression call = null;
            if (targetExpression == null)
            {
                call = Expression.Call(genericMethod, Expression.Constant(source), Expression.Constant(source), outerLambda, innerLambda, selectorResult);
            }
            else
            {
                call = Expression.Call(genericMethod, Expression.Constant(source), targetExpression, Expression.Constant(outerLambda), Expression.Constant(innerLambda), selectorResult);
            }

            return new KeyValuePair<string, Expression>(Name, call);
        }

        private static void AddTypes(Dictionary<string, Type> dic, string name, Type type, KeyValuePair<string, ICollection<string>> select)
        {
            if (select.IsEmpty())
            {
                return;
            }

            var propertyTypes = select.Value.Select(v =>
            {
                var prop = type.GetProperty(v);
                return new
                {
                    Name = name + "_" + prop.Name,
                    Type = prop.PropertyType
                };
            });

            if (!propertyTypes.Any())
            {
                dic.Add(name, type);
            }
            else
            {
                foreach (var propertyType in propertyTypes)
                {
                    dic.Add(propertyType.Name, propertyType.Type);
                }
            }
        }

        private static IEnumerable<Expression> GetParameterExpressions(ParameterExpression arg, KeyValuePair<string, ICollection<string>> parameter)
        {
            if (parameter.IsEmpty())
            {
                return null;
            }

            if (parameter.Value == null || !parameter.Value.Any())
            {
                return new Expression[] { arg };
            }

            var result = new List<Expression>();
            foreach(var parameterName in parameter.Value)
            {
                result.Add(Expression.Property(arg, parameterName));
            }

            return result;
        }
    }
}
