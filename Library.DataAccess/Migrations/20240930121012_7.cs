using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateReceived",
                table: "ItemCopies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateWithdrawn",
                table: "ItemCopies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "ItemCopies");

            migrationBuilder.DropColumn(
                name: "DateWithdrawn",
                table: "ItemCopies");
        }
    }
}
