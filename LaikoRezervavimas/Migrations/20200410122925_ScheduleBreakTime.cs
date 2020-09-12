using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class ScheduleBreakTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakEndTime",
                table: "EmployeeSchedules",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakStartTime",
                table: "EmployeeSchedules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakEndTime",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "BreakStartTime",
                table: "EmployeeSchedules");
        }
    }
}
