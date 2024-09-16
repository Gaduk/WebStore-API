using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_ordered_good_id_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderedGoods")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderedGoods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods",
                columns: new[] { "OrderId", "GoodId" });
        }
    }
}
