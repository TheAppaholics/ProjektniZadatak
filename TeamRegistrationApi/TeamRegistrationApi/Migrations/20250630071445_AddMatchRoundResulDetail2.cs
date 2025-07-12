using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamRegistrationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchRoundResulDetail2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScoreA = table.Column<int>(type: "INTEGER", nullable: false),
                    ScoreB = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamA = table.Column<string>(type: "TEXT", nullable: false),
                    TeamB = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResults", x => x.Id);
                });
        }
    }
}
