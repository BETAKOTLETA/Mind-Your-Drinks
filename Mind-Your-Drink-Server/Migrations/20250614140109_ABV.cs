using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mind_Your_Drink_Models.Migrations
{
    /// <inheritdoc />
    public partial class ABV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PercentageOfAlcohol",
                table: "UserDrinks",
                newName: "ABV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ABV",
                table: "UserDrinks",
                newName: "PercentageOfAlcohol");
        }
    }
}
