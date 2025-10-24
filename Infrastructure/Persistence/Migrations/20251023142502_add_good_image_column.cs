using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_good_image_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Goods",
                type: "bytea",
                nullable: true,
                defaultValue: null);

            migrationBuilder.UpdateData(
                table: "Goods",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Image",
                value: null);

            migrationBuilder.UpdateData(
                table: "Goods",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Image",
                value: null);

            migrationBuilder.UpdateData(
                table: "Goods",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Image",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Goods");
        }
    }
}