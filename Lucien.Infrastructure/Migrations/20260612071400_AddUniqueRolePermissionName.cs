using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lucien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueRolePermissionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId_Name",
                schema: "identity",
                table: "Permissions",
                columns: new[] { "RoleId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleId_Name",
                schema: "identity",
                table: "Permissions");
        }
    }
}
