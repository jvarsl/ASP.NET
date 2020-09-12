using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class AddedCreatedTimesToAppealAndNotiflication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeAppealSalons",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeAppealSalons");
        }
    }
}
