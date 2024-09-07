using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_navigation_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods");

            migrationBuilder.RenameColumn(
                name: "UserLogin",
                table: "Orders",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderedGoods",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4eb503c0-ac54-41d6-b451-38044164adec", "AQAAAAIAAYagAAAAEIK4kAGQL/h1gU7Be2h/1dFJy/Db74ZwP/HaHnL18lUtgntWgBHu/68BIceQzhc+2g==", "06861d69-abbf-4e3b-ac50-946948465d9d" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedGoods_GoodId",
                table: "OrderedGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedGoods_OrderId",
                table: "OrderedGoods",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedGoods_Goods_GoodId",
                table: "OrderedGoods",
                column: "GoodId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedGoods_Orders_OrderId",
                table: "OrderedGoods",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedGoods_Goods_GoodId",
                table: "OrderedGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedGoods_Orders_OrderId",
                table: "OrderedGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods");

            migrationBuilder.DropIndex(
                name: "IX_OrderedGoods_GoodId",
                table: "OrderedGoods");

            migrationBuilder.DropIndex(
                name: "IX_OrderedGoods_OrderId",
                table: "OrderedGoods");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderedGoods");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Orders",
                newName: "UserLogin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedGoods",
                table: "OrderedGoods",
                columns: new[] { "OrderId", "GoodId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd2408dc-22a7-41a8-bf57-63a63f9788eb", "AQAAAAIAAYagAAAAEDtEiPjzm1649kOOhjzQtKx9CLW4lbChj9+JX55hjzw9Q3GZFxG4GSSuNXUBOMMu7A==", "3a798554-bf07-40b7-a67b-010c45875360" });
        }
    }
}
