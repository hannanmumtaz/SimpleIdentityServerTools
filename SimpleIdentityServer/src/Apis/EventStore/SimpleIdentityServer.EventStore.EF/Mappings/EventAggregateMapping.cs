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

using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.EventStore.EF.Models;
using System;

namespace SimpleIdentityServer.EventStore.EF.Mappings
{
    internal static class EventAggregateMapping
    {
        public static ModelBuilder AddEventAggregate(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<EventAggregate>()
                .ToTable("events")
                .HasKey(e => e.Id);
            return modelBuilder;
        }
    }
}
