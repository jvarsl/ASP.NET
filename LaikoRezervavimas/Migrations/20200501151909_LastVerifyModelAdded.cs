using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class LastVerifyModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sp_Hours24Tables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour00 = table.Column<TimeSpan>(nullable: true),
                    Hour01 = table.Column<TimeSpan>(nullable: true),
                    Hour02 = table.Column<TimeSpan>(nullable: true),
                    Hour03 = table.Column<TimeSpan>(nullable: true),
                    Hour04 = table.Column<TimeSpan>(nullable: true),
                    Hour05 = table.Column<TimeSpan>(nullable: true),
                    Hour06 = table.Column<TimeSpan>(nullable: true),
                    Hour07 = table.Column<TimeSpan>(nullable: true),
                    Hour08 = table.Column<TimeSpan>(nullable: true),
                    Hour09 = table.Column<TimeSpan>(nullable: true),
                    Hour10 = table.Column<TimeSpan>(nullable: true),
                    Hour11 = table.Column<TimeSpan>(nullable: true),
                    Hour12 = table.Column<TimeSpan>(nullable: true),
                    Hour13 = table.Column<TimeSpan>(nullable: true),
                    Hour14 = table.Column<TimeSpan>(nullable: true),
                    Hour15 = table.Column<TimeSpan>(nullable: true),
                    Hour16 = table.Column<TimeSpan>(nullable: true),
                    Hour17 = table.Column<TimeSpan>(nullable: true),
                    Hour18 = table.Column<TimeSpan>(nullable: true),
                    Hour19 = table.Column<TimeSpan>(nullable: true),
                    Hour20 = table.Column<TimeSpan>(nullable: true),
                    Hour21 = table.Column<TimeSpan>(nullable: true),
                    Hour22 = table.Column<TimeSpan>(nullable: true),
                    Hour23 = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_Hours24Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sp_LastTimeChecks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Boolean = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_LastTimeChecks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sp_Hours24Tables");

            migrationBuilder.DropTable(
                name: "sp_LastTimeChecks");
        }
    }
}
