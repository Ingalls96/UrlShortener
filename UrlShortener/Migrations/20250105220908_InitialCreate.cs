using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Url",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LongUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ShortUrl = table.Column<string>(type: "TEXT", nullable: true),
                    GeneratedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SiteUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Url", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Url_SiteUser_SiteUserId",
                        column: x => x.SiteUserId,
                        principalTable: "SiteUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Url_SiteUserId",
                table: "Url",
                column: "SiteUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Url");

            migrationBuilder.DropTable(
                name: "SiteUser");
        }
    }
}
