using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS.Persistence.Migrations.TestTables
{
    /// <inheritdoc />
    public partial class AddAllowedTimeToTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowedExecutionTimeInSec",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedExecutionTimeInSec",
                table: "Tests");
        }
    }
}
