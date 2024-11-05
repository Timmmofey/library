using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class LibrarianConfiguration : IEntityTypeConfiguration<LibrarianEntity>
    {
        public void Configure(EntityTypeBuilder<LibrarianEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.ReadingRoom)
                .WithMany(i => i.Librarians);

            builder
                .HasMany(i => i.Loans)
                .WithOne(i => i.Librarian)
                .HasForeignKey(i => i.LibrarianId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
