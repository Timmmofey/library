using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class ItemCopyConfiguration : IEntityTypeConfiguration<ItemCopyEntity>
    {
          public void Configure(EntityTypeBuilder<ItemCopyEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.Item)
                .WithMany(i => i.ItemCopies);

            builder
                .HasOne(i => i.Shelf)
                .WithMany(i => i.ItemCopies);
        }
    }
