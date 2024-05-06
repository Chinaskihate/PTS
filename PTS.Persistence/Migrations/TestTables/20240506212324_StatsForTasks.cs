using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS.Persistence.Migrations.TestTables
{
    /// <inheritdoc />
    public partial class StatsForTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuccessfulSubmissionCount",
                table: "TestTaskVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSubmissionCount",
                table: "TestTaskVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuccessfulSubmissionCount",
                table: "TestTaskVersions");

            migrationBuilder.DropColumn(
                name: "TotalSubmissionCount",
                table: "TestTaskVersions");
        }
    }
}
