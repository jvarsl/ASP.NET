using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class LatLngAndStaffTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceCategoryId",
                table: "Services",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Salons",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Salons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeAppealSalons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    SalonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAppealSalons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAppealSalons_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAppealSalons_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffSalons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(nullable: false),
                    SalonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSalons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffSalons_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffSalons_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAppealSalons_EmployeeId",
                table: "EmployeeAppealSalons",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAppealSalons_SalonId",
                table: "EmployeeAppealSalons",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalons_SalonId",
                table: "StaffSalons",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalons_StaffId",
                table: "StaffSalons",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "EmployeeAppealSalons");

            migrationBuilder.DropTable(
                name: "StaffSalons");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Salons");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Salons");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceCategoryId",
                table: "Services",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategory_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
