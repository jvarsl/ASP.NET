using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class employepicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployeePictures_EmployeePictureId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeePictureId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeePictureId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeePictures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePictures_EmployeeId",
                table: "EmployeePictures",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePictures_AspNetUsers_EmployeeId",
                table: "EmployeePictures",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePictures_AspNetUsers_EmployeeId",
                table: "EmployeePictures");

            migrationBuilder.DropIndex(
                name: "IX_EmployeePictures_EmployeeId",
                table: "EmployeePictures");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeePictures");

            migrationBuilder.AddColumn<int>(
                name: "EmployeePictureId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeePictureId",
                table: "AspNetUsers",
                column: "EmployeePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployeePictures_EmployeePictureId",
                table: "AspNetUsers",
                column: "EmployeePictureId",
                principalTable: "EmployeePictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
