using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamRegistrationApi.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CreateMatchResultsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamA = table.Column<string>(type: "TEXT", nullable: false),
                    TeamB = table.Column<string>(type: "TEXT", nullable: false),
                    ScoreA = table.Column<int>(type: "INTEGER", nullable: false),
                    ScoreB = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchResults");
        }
    }
}
