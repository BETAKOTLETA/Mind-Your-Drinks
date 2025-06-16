using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mind_Your_Drink_Models.Migrations
{
    /// <inheritdoc />
    public partial class Abv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ABV",
                table: "UserDrinks",
                newName: "Abv");

            migrationBuilder.RenameColumn(
                name: "Callories",
                table: "UserDrinks",
                newName: "Calories");

            migrationBuilder.AlterColumn<float>(
                name: "Abv",
                table: "UserDrinks",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Abv",
                table: "UserDrinks",
                newName: "ABV");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "UserDrinks",
                newName: "Callories");

            migrationBuilder.AlterColumn<int>(
                name: "ABV",
                table: "UserDrinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
