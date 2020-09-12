using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class serviceCategoryFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategory_Services_ServiceId",
                table: "ServiceCategory");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCategory_ServiceId",
                table: "ServiceCategory");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ServiceCategory");

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "Services",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceCategoryId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ServiceCategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCategory_ServiceId",
                table: "ServiceCategory",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategory_Services_ServiceId",
                table: "ServiceCategory",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
