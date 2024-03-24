using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS.Persistence.Migrations.TestTables
{
    /// <inheritdoc />
    public partial class AddTaskIdPropertyToTaskVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "TestTaskVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "TestTaskVersions");
        }
    }
}
