﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleIdentityServer.Configuration.EF.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticationProviders",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CallbackPath = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Namespace = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationProviders", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AuthenticationProviderId = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_AuthenticationProviders_AuthenticationProviderId",
                        column: x => x.AuthenticationProviderId,
                        principalTable: "AuthenticationProviders",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_AuthenticationProviderId",
                table: "Options",
                column: "AuthenticationProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "AuthenticationProviders");
        }
    }
}
