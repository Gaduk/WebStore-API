using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_admin_claims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "431d6728-209c-4fac-8693-b8747b7bdc8d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin", 0, "2e89036d-7391-4a29-b6d7-dcbc95041e38", "admin@mail.ru", true, "Иван", false, "Иванов", false, null, "ADMIN@MAIL.RU", "ADMIN", "AQAAAAIAAYagAAAAEOcZj1TqZJsmFgQ+PuXAeNdeAhkxDBT0K5o/YZuxs8HH5WlHOtPUzu39RZqCOYCY9g==", null, false, "18d74696-c8ed-4601-95c0-71f09c7130dd", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "user", "admin" },
                    { 2, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin", "admin" },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "431d6728-209c-4fac-8693-b8747b7bdc8d", 0, "a5297634-4332-46cb-8b7f-45fb909ac2b9", "admin@mail.ru", true, "Иван", false, "Иванов", false, null, "ADMIN@MAIL.RU", "ADMIN", "AQAAAAIAAYagAAAAEJ9USPM9ixsAq75C6m+NBNyQ+wbwm/zdVTIenHKMoVHg1GaLHn9kPTxJe6YValYbcQ==", null, false, "95e1dc93-6a1f-426e-8a4c-0bfefd1f8db3", false, "admin" });
        }
    }
}
