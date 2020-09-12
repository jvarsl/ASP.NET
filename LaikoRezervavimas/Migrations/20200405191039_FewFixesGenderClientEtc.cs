using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class FewFixesGenderClientEtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_EmployeeId",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "EmailPromotion",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Services",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeSchedules",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_EmployeeId",
                table: "EmployeeSchedules",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_EmployeeId",
                table: "EmployeeSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeSchedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "EmailPromotion",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_EmployeeId",
                table: "EmployeeSchedules",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
