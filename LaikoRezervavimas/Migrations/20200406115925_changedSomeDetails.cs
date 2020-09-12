using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class changedSomeDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ReservationComments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ReservationComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
