using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;
public class ItemConfiguration: IEntityTypeConfiguration<ItemEntity>
    {
        public void Configure(EntityTypeBuilder<ItemEntity> builder) 
        { 
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.Category)
                .WithMany(i => i.Items);

            builder
                .HasMany(i => i.ItemCopies)
                .WithOne(c => c.Item)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(i => i.Authors)
                .WithMany(i => i.Items);
        }
    }
