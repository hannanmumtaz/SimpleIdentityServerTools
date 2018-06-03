﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SimpleIdentityServer.Configuration.EF;

namespace SimpleIdentityServer.Configuration.EF.Migrations
{
    [DbContext(typeof(SimpleIdentityServerConfigurationContext))]
    partial class SimpleIdentityServerConfigurationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SimpleIdentityServer.Configuration.EF.Models.AuthenticationProvider", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("CallbackPath");

                    b.Property<string>("ClassName");

                    b.Property<string>("Code");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Namespace");

                    b.Property<int>("Type");

                    b.HasKey("Name");

                    b.ToTable("AuthenticationProviders");
                });

            modelBuilder.Entity("SimpleIdentityServer.Configuration.EF.Models.Option", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("AuthenticationProviderId");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AuthenticationProviderId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("SimpleIdentityServer.Configuration.EF.Models.Setting", b =>
                {
                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Key");

                    b.ToTable("setting");
                });

            modelBuilder.Entity("SimpleIdentityServer.Configuration.EF.Models.Option", b =>
                {
                    b.HasOne("SimpleIdentityServer.Configuration.EF.Models.AuthenticationProvider", "AuthenticationProvider")
                        .WithMany("Options")
                        .HasForeignKey("AuthenticationProviderId");
                });
        }
    }
}
