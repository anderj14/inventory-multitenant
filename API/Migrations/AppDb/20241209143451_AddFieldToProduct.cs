using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddFieldToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "4581a20e-beca-43b4-a812-6b378c7f143d");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8fb9fb8b-158d-4cc8-af30-30121975652e");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a8debc18-50a3-47bf-b37a-fe747cd5fd6a");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4581a20e-beca-43b4-a812-6b378c7f143d", null, "Member", "MEMBER" },
                    { "8fb9fb8b-158d-4cc8-af30-30121975652e", null, "SuperAdmin", "SUPERADMIN" },
                    { "a8debc18-50a3-47bf-b37a-fe747cd5fd6a", null, "Admin", "ADMIN" }
                });
        }
    }
}
