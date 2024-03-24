using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS.Persistence.Migrations.TaskTables
{
    /// <inheritdoc />
    public partial class AddCodeTemplateToVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeTemplate",
                table: "TaskVersions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeTemplate",
                table: "TaskVersions");
        }
    }
}
