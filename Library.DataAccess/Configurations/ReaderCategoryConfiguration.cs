using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class ReaderCategoryConfiguration : IEntityTypeConfiguration<ReaderCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ReaderCategoryEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
            .HasMany(i => i.Readers)
            .WithOne(i => i.ReaderCategory)
            .HasForeignKey(i => i.ReaderCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
