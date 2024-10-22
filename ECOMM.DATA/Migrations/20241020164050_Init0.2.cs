using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "ParentCategoryTag");

            migrationBuilder.RenameColumn(
                name: "CategoryTag",
                table: "Categories",
                newName: "ParentCategoryDescription");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySubCategory",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySubCategory", x => new { x.CategoriesId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_SubCategory_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "SubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategory_SubCategoriesId",
                table: "CategorySubCategory",
                column: "SubCategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySubCategory");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryTag",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryDescription",
                table: "Categories",
                newName: "CategoryTag");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
