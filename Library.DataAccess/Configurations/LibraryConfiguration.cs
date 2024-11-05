using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<LibraryEntity>
    {
        public void Configure(EntityTypeBuilder<LibraryEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasMany(i => i.ReadingRooms)
                .WithOne(i => i.Library)
                .HasForeignKey(i => i.LibraryId)
                .OnDelete(DeleteBehavior.SetNull);


            builder
                .HasMany(i => i.Readers)
                .WithOne(i => i.Library)
                .HasForeignKey(i => i.LibraryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
