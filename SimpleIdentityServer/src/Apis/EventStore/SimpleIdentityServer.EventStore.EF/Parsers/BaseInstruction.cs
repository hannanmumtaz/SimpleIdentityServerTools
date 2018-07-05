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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleIdentityServer.EventStore.EF.Parsers
{
    public abstract class BaseInstruction
    {
        protected BaseInstruction SubInstruction;
        protected BaseInstruction TargetInstruction;
        protected string Parameter;

        public BaseInstruction() { }

        public void SetSubInstruction(BaseInstruction instruction)
        {
            if (instruction == null)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            if (SubInstruction != null)
            {
                throw new InvalidOperationException("only one sub instruction can be set");
            }

            SubInstruction = instruction;
        }

        public void SetTargetInstruction(BaseInstruction instruction)
        {
            if(instruction == null)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            if (TargetInstruction != null)
            {
                throw new InvalidOperationException("only one target instruction can be set");
            }

            TargetInstruction = instruction;
        }

        public void SetParameter(string parameter)
        {
            Parameter = parameter;
        }

        public string GetParameter()
        {
            return Parameter;
        }

        public BaseInstruction GetSubInstruction()
        {
            return SubInstruction;
        }

        public BaseInstruction GetTargetInstruction()
        {
            return TargetInstruction;
        }

        public abstract KeyValuePair<string, Expression>? GetExpression<TSource>(Type sourceType, ParameterExpression rootParameter, IEnumerable<TSource> source);
    }
}
