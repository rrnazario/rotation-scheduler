using Microsoft.EntityFrameworkCore.Migrations;
using Rotation.Application.Features.Activities;

#nullable disable

namespace Rotation.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ActivityResume>(
                name: "Resume",
                table: "Activity",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Activity");
        }
    }
}
