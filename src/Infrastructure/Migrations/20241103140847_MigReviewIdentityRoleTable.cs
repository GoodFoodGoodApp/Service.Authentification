using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigReviewIdentityRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("bc64abd3-d27c-450a-8256-6a4cf019b5a7"), null, "Role", "User", "USER" },
                    { new Guid("d9495c85-ccb9-4e17-9e0d-ce45f8157c4e"), null, "Role", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("587d0161-2c2d-4c82-a096-78791d6b0f29"), 0, null, null, "7eff9b0a-2884-4f3a-ac5f-2c11ef5a5d0c", null, "admin@example.com", true, null, null, true, null, "ADMIN@EXAMPLE.COM", null, "AQAAAAIAAYagAAAAEPpKWLczWuLtepyE5krvNn0XKz5HbTg490Z9G/fXTqijstWWzNC5fChJOv7FGmX+aA==", null, true, null, "2848b590-8f4c-4f0e-a61a-a615101a7bec", false, "admin@example.com" },
                    { new Guid("68b3f1ad-ca3c-43c4-9f13-82d61ff3eac8"), 0, null, null, "e404bf93-60f9-4822-8476-658e46ac7e97", null, "user@example.com", true, null, null, true, null, "USER@EXAMPLE.COM", null, "AQAAAAIAAYagAAAAEN2NrMCajGzUuaDRsV6/vJf/URIOpPhtSYLhK1TUBQX3sMHaHdkYPDMsOCsa0oMpnA==", null, true, null, "ba744963-5a86-4901-b0f9-4c6859c3466a", false, "user@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bc64abd3-d27c-450a-8256-6a4cf019b5a7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d9495c85-ccb9-4e17-9e0d-ce45f8157c4e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("587d0161-2c2d-4c82-a096-78791d6b0f29"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("68b3f1ad-ca3c-43c4-9f13-82d61ff3eac8"));

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");
        }
    }
}
