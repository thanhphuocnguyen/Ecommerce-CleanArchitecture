using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldForPermTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetRoleClaims",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetRoleClaims");
        }
    }
}