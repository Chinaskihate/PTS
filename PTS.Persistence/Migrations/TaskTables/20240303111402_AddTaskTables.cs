﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PTS.Persistence.Migrations.TaskTables
{
    /// <inheritdoc />
    public partial class AddTaskTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Complexity = table.Column<int>(type: "integer", nullable: false),
                    AvgTimeInMin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_Themes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Themes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskTheme",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "integer", nullable: false),
                    ThemesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTheme", x => new { x.TasksId, x.ThemesId });
                    table.ForeignKey(
                        name: "FK_TaskTheme_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTheme_Themes_ThemesId",
                        column: x => x.ThemesId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "IsBanned", "Name", "ParentId" },
                values: new object[] { 1, false, "RootTheme", null });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTheme_ThemesId",
                table: "TaskTheme",
                column: "ThemesId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_ParentId",
                table: "Themes",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTheme");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
