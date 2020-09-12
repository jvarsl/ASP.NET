using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class CityFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobTitles_JobTitleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceReservations_Reservations_ReservationId",
                table: "ServiceReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceReservations_Services_ServiceId",
                table: "ServiceReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "ServiceReservations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "ServiceReservations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salons_CityId",
                table: "Salons",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobTitles_JobTitleId",
                table: "AspNetUsers",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salons_Cities_CityId",
                table: "Salons",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceReservations_Reservations_ReservationId",
                table: "ServiceReservations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceReservations_Services_ServiceId",
                table: "ServiceReservations",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobTitles_JobTitleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Salons_Cities_CityId",
                table: "Salons");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceReservations_Reservations_ReservationId",
                table: "ServiceReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceReservations_Services_ServiceId",
                table: "ServiceReservations");

            migrationBuilder.DropIndex(
                name: "IX_Salons_CityId",
                table: "Salons");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "ServiceReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "ServiceReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobTitles_JobTitleId",
                table: "AspNetUsers",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceReservations_Reservations_ReservationId",
                table: "ServiceReservations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceReservations_Services_ServiceId",
                table: "ServiceReservations",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
