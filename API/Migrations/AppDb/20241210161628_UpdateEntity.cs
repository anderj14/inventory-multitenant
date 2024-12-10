using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class UpdateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "278ee299-0cf3-42ca-91b6-6319cbf02534");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "308f19f3-b3c4-4798-92d4-0b6f5ffa17a7");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "ee0a1c9d-febb-4ea8-b73e-64a38a56ac69");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e5c1fa8-f54f-4cb7-b688-19727f7d4bec", null, "Admin", "ADMIN" },
                    { "7bbbe6b6-8d07-4b55-8320-24a3f3a5fa19", null, "Member", "MEMBER" },
                    { "bbca2649-e760-4927-9798-2a737fe635b4", null, "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "2e5c1fa8-f54f-4cb7-b688-19727f7d4bec");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "7bbbe6b6-8d07-4b55-8320-24a3f3a5fa19");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bbca2649-e760-4927-9798-2a737fe635b4");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "278ee299-0cf3-42ca-91b6-6319cbf02534", null, "Member", "MEMBER" },
                    { "308f19f3-b3c4-4798-92d4-0b6f5ffa17a7", null, "Admin", "ADMIN" },
                    { "ee0a1c9d-febb-4ea8-b73e-64a38a56ac69", null, "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
