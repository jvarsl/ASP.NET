using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class ManagementPagesServiceIdKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Salons_SalonId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "SalonId",
                table: "Services",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Salons_SalonId",
                table: "Services",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Salons_SalonId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "SalonId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Salons_SalonId",
                table: "Services",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
