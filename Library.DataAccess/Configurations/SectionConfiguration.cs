using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<SectionEntity>
    {
        public void Configure(EntityTypeBuilder<SectionEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.ReadingRoom)
                .WithMany(i => i.Sections);

            builder
                .HasMany(i => i.Shelves)
                .WithOne(i => i.Section)
                .HasForeignKey(i => i.SectionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
