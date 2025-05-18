using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lucien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashUserstbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "identity",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "identity",
                table: "Users");
        }
    }
}
