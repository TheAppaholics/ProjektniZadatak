using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamRegistrationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentAndPlayerAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TournamentName",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentName",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Players");
        }
    }
}
