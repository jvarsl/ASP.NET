using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class notiflicationsAndTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "CK_Services_Gender_Enum_Constraint",
                table: "Services",
                sql: "[Gender] IN(0, 1, 2)");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_AspNetUsers_Gender_Enum_Constraint",
                table: "AspNetUsers",
                sql: "[Gender] IN(0, 1, 2)");

            migrationBuilder.AlterColumn<short>(
                name: "EstimatedTime",
                table: "Services",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Salons",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.CheckConstraint("CK_Notifications_Type_Enum_Constraint", "[Type] IN(0, 1)");
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Services_Gender_Enum_Constraint",
                table: "Services");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AspNetUsers_Gender_Enum_Constraint",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Salons");

            migrationBuilder.AlterColumn<byte>(
                name: "EstimatedTime",
                table: "Services",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
