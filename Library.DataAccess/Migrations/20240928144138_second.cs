using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_Libraries_LibraryId",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Libraries_LibraryId",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Sections",
                newName: "ReadingRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_LibraryId",
                table: "Sections",
                newName: "IX_Sections_ReadingRoomId");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Librarians",
                newName: "ReadingRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Librarians_LibraryId",
                table: "Librarians",
                newName: "IX_Librarians_ReadingRoomId");

            migrationBuilder.CreateTable(
                name: "ReadingRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LibraryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingRooms_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingRooms_LibraryId",
                table: "ReadingRooms",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "ReadingRooms");

            migrationBuilder.RenameColumn(
                name: "ReadingRoomId",
                table: "Sections",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_ReadingRoomId",
                table: "Sections",
                newName: "IX_Sections_LibraryId");

            migrationBuilder.RenameColumn(
                name: "ReadingRoomId",
                table: "Librarians",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Librarians_ReadingRoomId",
                table: "Librarians",
                newName: "IX_Librarians_LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_Libraries_LibraryId",
                table: "Librarians",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Libraries_LibraryId",
                table: "Sections",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
