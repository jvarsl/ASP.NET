using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class addedIsWorkingToEmployeeSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "IsTakingBreak",
                table: "EmployeeSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTakingBreak",
                table: "EmployeeSchedules");

        }
    }
}
