﻿#region copyright
// Copyright 2015 Habart Thierry
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
using SimpleIdentityServer.Configuration.EF.Mappings;
using SimpleIdentityServer.Configuration.EF.Models;

namespace SimpleIdentityServer.Configuration.EF
{
    public class SimpleIdentityServerConfigurationContext : DbContext
    {
        #region Constructor

        public SimpleIdentityServerConfigurationContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        #endregion

        #region Properties

        public virtual DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }

        #endregion

        #region Protected methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAuthenticationProviderMappings();
            modelBuilder.AddOptionMappings();
            modelBuilder.AddSettingMappings();
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
