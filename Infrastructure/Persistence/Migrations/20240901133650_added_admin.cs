using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "431d6728-209c-4fac-8693-b8747b7bdc8d", 0, "a5297634-4332-46cb-8b7f-45fb909ac2b9", "admin@mail.ru", true, "Иван", false, "Иванов", false, null, "ADMIN@MAIL.RU", "ADMIN", "AQAAAAIAAYagAAAAEJ9USPM9ixsAq75C6m+NBNyQ+wbwm/zdVTIenHKMoVHg1GaLHn9kPTxJe6YValYbcQ==", null, false, "95e1dc93-6a1f-426e-8a4c-0bfefd1f8db3", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "431d6728-209c-4fac-8693-b8747b7bdc8d");
        }
    }
}
