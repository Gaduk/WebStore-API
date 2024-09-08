using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updated_config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserLogin",
                table: "Orders",
                newName: "UserId"
            );
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e68fb2be-5f7a-40ba-8947-b1b195c3ef82", "AQAAAAIAAYagAAAAEHG1W6MylQcjbF7acVreojNZLf5IjsN5EgnObFuNBfFhEMnxFPPd2fqIjGpULh3QDQ==", "a24c3f06-bf09-4df9-b069-bf8a8b1a6b9f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserLogin"
            );
            
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60759d5a-2658-436c-9fd5-32ab215e5306", "AQAAAAIAAYagAAAAEBa4Ho0svW9rQX+s38mSr4bMKoD/OwxplecASbWTU4EK+6vYuLaukpTqv7Kf8VnJBQ==", "fe7eac7d-a61d-4a4d-a018-7c95df4afc2e" });
        }
    }
}
