using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCopies_Items_ItemId",
                table: "ItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemCopies_Shelfs_ShelfId",
                table: "ItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Librarians_LibrarianId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Readers_ReaderId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Libraries_LibraryId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_ReaderCategories_ReaderCategoryId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingRooms_Libraries_LibraryId",
                table: "ReadingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelfs_Sections_SectionId",
                table: "Shelfs");

            migrationBuilder.AlterColumn<string>(
                name: "PublicationDate",
                table: "Items",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCopies_Items_ItemId",
                table: "ItemCopies",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCopies_Shelfs_ShelfId",
                table: "ItemCopies",
                column: "ShelfId",
                principalTable: "Shelfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Librarians_LibrarianId",
                table: "Loans",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Readers_ReaderId",
                table: "Loans",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Libraries_LibraryId",
                table: "Readers",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_ReaderCategories_ReaderCategoryId",
                table: "Readers",
                column: "ReaderCategoryId",
                principalTable: "ReaderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingRooms_Libraries_LibraryId",
                table: "ReadingRooms",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelfs_Sections_SectionId",
                table: "Shelfs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCopies_Items_ItemId",
                table: "ItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemCopies_Shelfs_ShelfId",
                table: "ItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Librarians_LibrarianId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Readers_ReaderId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Libraries_LibraryId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_ReaderCategories_ReaderCategoryId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingRooms_Libraries_LibraryId",
                table: "ReadingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelfs_Sections_SectionId",
                table: "Shelfs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCopies_Items_ItemId",
                table: "ItemCopies",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCopies_Shelfs_ShelfId",
                table: "ItemCopies",
                column: "ShelfId",
                principalTable: "Shelfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_ReadingRooms_ReadingRoomId",
                table: "Librarians",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Librarians_LibrarianId",
                table: "Loans",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Readers_ReaderId",
                table: "Loans",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Libraries_LibraryId",
                table: "Readers",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_ReaderCategories_ReaderCategoryId",
                table: "Readers",
                column: "ReaderCategoryId",
                principalTable: "ReaderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingRooms_Libraries_LibraryId",
                table: "ReadingRooms",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ReadingRooms_ReadingRoomId",
                table: "Sections",
                column: "ReadingRoomId",
                principalTable: "ReadingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelfs_Sections_SectionId",
                table: "Shelfs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
