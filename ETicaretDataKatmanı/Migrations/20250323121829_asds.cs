using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretDataKatmanı.Migrations
{
    public partial class asds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Orders");
        }
    }
}
