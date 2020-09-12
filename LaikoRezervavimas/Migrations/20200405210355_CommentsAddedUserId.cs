using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class CommentsAddedUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "ReservationComments",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationComments_AppUserId",
                table: "ReservationComments",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationComments_AspNetUsers_AppUserId",
                table: "ReservationComments");

            migrationBuilder.DropIndex(
                name: "IX_ReservationComments_AppUserId",
                table: "ReservationComments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ReservationComments");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
