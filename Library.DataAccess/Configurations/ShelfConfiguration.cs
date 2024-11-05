using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class ShelfConfiguration : IEntityTypeConfiguration<ShelfEntity>
    {
        public void Configure(EntityTypeBuilder<ShelfEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.Section)
                .WithMany(i => i.Shelves);

            builder
                .HasMany(i => i.ItemCopies)
                .WithOne(i => i.Shelf)
                .HasForeignKey(i => i.ShelfId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
