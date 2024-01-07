using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniFi.Data.Migrations
{
    public partial class AddVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage1",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundVideo",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImage1",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "BackgroundVideo",
                table: "Brand");
        }
    }
}
