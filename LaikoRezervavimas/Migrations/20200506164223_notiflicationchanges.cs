using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonWithRazor.Migrations
{
    public partial class notiflicationchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Notifications_Type_Enum_Constraint",
                table: "Notifications");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Notifications_Type_Enum_Constraint",
                table: "Notifications",
                sql: "[Type] IN(0, 1, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Notifications_Type_Enum_Constraint",
                table: "Notifications");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Notifications_Type_Enum_Constraint",
                table: "Notifications",
                sql: "[Type] IN(0, 1)");
        }
    }
}
