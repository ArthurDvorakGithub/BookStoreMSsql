using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreApp.Migrations
{
    public partial class addShopCartAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExistInLocalDb",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExistInLocalDb",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
