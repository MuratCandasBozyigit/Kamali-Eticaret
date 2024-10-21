using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorySubCategory_SubCategory_SubCategoriesId",
                table: "CategorySubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategory",
                table: "SubCategory");

            migrationBuilder.RenameTable(
                name: "SubCategory",
                newName: "SubCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySubCategory_SubCategories_SubCategoriesId",
                table: "CategorySubCategory",
                column: "SubCategoriesId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorySubCategory_SubCategories_SubCategoriesId",
                table: "CategorySubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories");

            migrationBuilder.RenameTable(
                name: "SubCategories",
                newName: "SubCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategory",
                table: "SubCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySubCategory_SubCategory_SubCategoriesId",
                table: "CategorySubCategory",
                column: "SubCategoriesId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
