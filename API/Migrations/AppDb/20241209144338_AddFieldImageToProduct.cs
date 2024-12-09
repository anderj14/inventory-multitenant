using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddFieldImageToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "02e60541-d5c1-4faf-8d1e-0f9c3bf9311d");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "4ff7cb0e-3d36-4118-b167-1eb8db0c77c7");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6ac4ab16-d3db-4b9a-acac-f42355a31c84");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02e60541-d5c1-4faf-8d1e-0f9c3bf9311d", null, "Admin", "ADMIN" },
                    { "4ff7cb0e-3d36-4118-b167-1eb8db0c77c7", null, "Member", "MEMBER" },
                    { "6ac4ab16-d3db-4b9a-acac-f42355a31c84", null, "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
