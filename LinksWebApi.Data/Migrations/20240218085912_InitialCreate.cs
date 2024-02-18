using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinksWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginRelativeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedOriginRelativeUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedirectionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmartLinkId = table.Column<int>(type: "int", nullable: false),
                    RuleType = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntervalStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IntervalEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedirectionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedirectionRules_SmartLinks_SmartLinkId",
                        column: x => x.SmartLinkId,
                        principalTable: "SmartLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedirectionRules_SmartLinkId",
                table: "RedirectionRules",
                column: "SmartLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartLinks_NormalizedOriginRelativeUrl",
                table: "SmartLinks",
                column: "NormalizedOriginRelativeUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedirectionRules");

            migrationBuilder.DropTable(
                name: "SmartLinks");
        }
    }
}
