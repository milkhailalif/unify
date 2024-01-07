using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniFi.Data.Migrations
{
    public partial class InitialProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AffiliateBrandLinks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AffiliateBrandLinks");
        }
    }
}
