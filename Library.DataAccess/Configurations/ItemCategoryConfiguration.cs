using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ItemCategoryEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasMany(i => i.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
