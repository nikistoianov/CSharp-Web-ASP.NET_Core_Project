using Microsoft.EntityFrameworkCore.Migrations;

namespace WCR.Data.Migrations
{
    public partial class UniqueUserShortname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShortName",
                table: "AspNetUsers",
                column: "ShortName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShortName",
                table: "AspNetUsers");
        }
    }
}
