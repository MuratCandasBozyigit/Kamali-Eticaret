using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class InıtMultipleImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath3",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImagePath2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImagePath3",
                table: "Products");
        }
    }
}
