using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaegoryName",
                table: "Categories",
                newName: "CategoryTag");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryTag",
                table: "Categories",
                newName: "CaegoryName");
        }
    }
}
