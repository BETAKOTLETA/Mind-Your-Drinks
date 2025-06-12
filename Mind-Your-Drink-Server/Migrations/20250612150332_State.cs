using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mind_Your_Drink_Server.Migrations
{
    /// <inheritdoc />
    public partial class State : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateName",
                table: "Users");
        }
    }
}
