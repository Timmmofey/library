﻿// <auto-generated />
using System;
using Library.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Library.DataAccess.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20240929213002_5")]
    partial class _5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthorEntityItemEntity", b =>
                {
                    b.Property<Guid>("AuthorsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ItemsId")
                        .HasColumnType("uuid");

                    b.HasKey("AuthorsId", "ItemsId");

                    b.HasIndex("ItemsId");

                    b.ToTable("AuthorEntityItemEntity");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.AuthorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemCategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemCopyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("InventoryNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Loanable")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ShelfId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ShelfId");

                    b.ToTable("ItemCopies");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("PublicationDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ReadingRoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReadingRoomId");

                    b.ToTable("Librarians");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LibraryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LoanEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ItemCopyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LibrarianId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReaderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ItemCopyId");

                    b.HasIndex("LibrarianId");

                    b.HasIndex("ReaderId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReaderCategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ReaderCategories");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReaderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Course")
                        .HasColumnType("text");

                    b.Property<string>("EducationalInstitution")
                        .HasColumnType("text");

                    b.Property<string>("Faculty")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GroupNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Organization")
                        .HasColumnType("text");

                    b.Property<Guid>("ReaderCategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("ResearchTopic")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SubscriptionEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.HasIndex("ReaderCategoryId");

                    b.ToTable("Readers");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReadingRoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("ReadingRooms");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.SectionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ReadingRoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReadingRoomId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ShelfEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Shelfs");
                });

            modelBuilder.Entity("AuthorEntityItemEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.AuthorEntity", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.DataAccess.Entities.ItemEntity", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemCopyEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.ItemEntity", "Item")
                        .WithMany("ItemCopies")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Library.DataAccess.Entities.ShelfEntity", "Shelf")
                        .WithMany("ItemCopies")
                        .HasForeignKey("ShelfId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Shelf");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.ItemCategoryEntity", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.ReadingRoomEntity", "ReadingRoom")
                        .WithMany("Librarians")
                        .HasForeignKey("ReadingRoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("ReadingRoom");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LoanEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.ItemCopyEntity", "ItemCopy")
                        .WithMany("Loans")
                        .HasForeignKey("ItemCopyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.DataAccess.Entities.LibrarianEntity", "Librarian")
                        .WithMany("Loans")
                        .HasForeignKey("LibrarianId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Library.DataAccess.Entities.ReaderEntity", "Reader")
                        .WithMany("Loans")
                        .HasForeignKey("ReaderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("ItemCopy");

                    b.Navigation("Librarian");

                    b.Navigation("Reader");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReaderEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.LibraryEntity", "Library")
                        .WithMany("Readers")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Library.DataAccess.Entities.ReaderCategoryEntity", "ReaderCategory")
                        .WithMany("Readers")
                        .HasForeignKey("ReaderCategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Library");

                    b.Navigation("ReaderCategory");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReadingRoomEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.LibraryEntity", "Library")
                        .WithMany("ReadingRooms")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.SectionEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.ReadingRoomEntity", "ReadingRoom")
                        .WithMany("Sections")
                        .HasForeignKey("ReadingRoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("ReadingRoom");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ShelfEntity", b =>
                {
                    b.HasOne("Library.DataAccess.Entities.SectionEntity", "Section")
                        .WithMany("Shelves")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemCategoryEntity", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemCopyEntity", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ItemEntity", b =>
                {
                    b.Navigation("ItemCopies");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.LibraryEntity", b =>
                {
                    b.Navigation("Readers");

                    b.Navigation("ReadingRooms");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReaderCategoryEntity", b =>
                {
                    b.Navigation("Readers");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReaderEntity", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ReadingRoomEntity", b =>
                {
                    b.Navigation("Librarians");

                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.SectionEntity", b =>
                {
                    b.Navigation("Shelves");
                });

            modelBuilder.Entity("Library.DataAccess.Entities.ShelfEntity", b =>
                {
                    b.Navigation("ItemCopies");
                });
#pragma warning restore 612, 618
        }
    }
}
