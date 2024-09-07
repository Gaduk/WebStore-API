using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_config_classes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "IsAdmin", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd2408dc-22a7-41a8-bf57-63a63f9788eb", true, "AQAAAAIAAYagAAAAEDtEiPjzm1649kOOhjzQtKx9CLW4lbChj9+JX55hjzw9Q3GZFxG4GSSuNXUBOMMu7A==", "3a798554-bf07-40b7-a67b-010c45875360" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "IsAdmin", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e89036d-7391-4a29-b6d7-dcbc95041e38", false, "AQAAAAIAAYagAAAAEOcZj1TqZJsmFgQ+PuXAeNdeAhkxDBT0K5o/YZuxs8HH5WlHOtPUzu39RZqCOYCY9g==", "18d74696-c8ed-4601-95c0-71f09c7130dd" });
        }
    }
}
