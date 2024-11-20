using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Denemeem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShippingInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "ShippingInfo",
                newName: "ShippingCity");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "ShippingInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingInfoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShippingInfoId",
                table: "AspNetUsers",
                column: "ShippingInfoId",
                unique: true,
                filter: "[ShippingInfoId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShippingInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                table: "ShippingInfo",
                newName: "City");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingInfoId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShippingInfoId",
                table: "AspNetUsers",
                column: "ShippingInfoId",
                unique: true);
        }
    }
}
