using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFood.Data.Migrations
{
    public partial class removealternativekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Positions_Name",
                table: "Positions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Items_Name",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Positions_Name",
                table: "Positions",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Items_Name",
                table: "Items",
                column: "Name");
        }
    }
}
