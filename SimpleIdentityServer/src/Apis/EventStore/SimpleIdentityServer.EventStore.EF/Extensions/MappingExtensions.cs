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

using SimpleIdentityServer.EventStore.EF.Models;
using System;
using Domain = SimpleIdentityServer.EventStore.Core.Models;

namespace SimpleIdentityServer.EventStore.EF.Extensions
{
    internal static class MappingExtensions
    {
        public static Domain.EventAggregate ToDomain(this EventAggregate evtAggregate)
        {
            if (evtAggregate == null)
            {
                throw new ArgumentNullException(nameof(evtAggregate));
            }

            return new Domain.EventAggregate
            {
                Id = evtAggregate.Id,
                AggregateId = evtAggregate.AggregateId,
                Order = evtAggregate.Order,
                CreatedOn = evtAggregate.CreatedOn,
                Description = evtAggregate.Description,
                Payload = evtAggregate.Payload,
                Type = evtAggregate.Type,
                Key = evtAggregate.Key,
                Verbosity = (Domain.EventVerbosities)evtAggregate.Verbosity
            };
        }

        public static EventAggregate ToModel(this Domain.EventAggregate evtAggregate)
        {
            if (evtAggregate == null)
            {
                throw new ArgumentNullException(nameof(evtAggregate));
            }

            return new EventAggregate
            {
                Id = evtAggregate.Id,
                AggregateId = evtAggregate.AggregateId,
                CreatedOn = evtAggregate.CreatedOn,
                Order = evtAggregate.Order,
                Description = evtAggregate.Description,
                Payload = evtAggregate.Payload,
                Type = evtAggregate.Type,
                Key = evtAggregate.Key,
                Verbosity = (int)evtAggregate.Verbosity
            };
        }
    }
}
