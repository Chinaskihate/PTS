using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS.Persistence.Migrations.TaskTables
{
    /// <inheritdoc />
    public partial class AddVersionToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskVersion_Tasks_TaskId",
                table: "TaskVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskVersion",
                table: "TaskVersion");

            migrationBuilder.RenameTable(
                name: "TaskVersion",
                newName: "TaskVersions");

            migrationBuilder.RenameIndex(
                name: "IX_TaskVersion_TaskId",
                table: "TaskVersions",
                newName: "IX_TaskVersions_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskVersions",
                table: "TaskVersions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskVersions_Tasks_TaskId",
                table: "TaskVersions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskVersions_Tasks_TaskId",
                table: "TaskVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskVersions",
                table: "TaskVersions");

            migrationBuilder.RenameTable(
                name: "TaskVersions",
                newName: "TaskVersion");

            migrationBuilder.RenameIndex(
                name: "IX_TaskVersions_TaskId",
                table: "TaskVersion",
                newName: "IX_TaskVersion_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskVersion",
                table: "TaskVersion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskVersion_Tasks_TaskId",
                table: "TaskVersion",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
