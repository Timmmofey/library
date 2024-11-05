using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class ReaderConfiguration : IEntityTypeConfiguration<ReaderEntity>
    {
        public void Configure(EntityTypeBuilder<ReaderEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
            .HasOne(i => i.ReaderCategory)
            .WithMany(i => i.Readers);

            builder
                .HasOne(i => i.Library)
                .WithMany(i => i.Readers);

            builder
                .HasMany(i => i.Loans)
                .WithOne(i => i.Reader)
                .HasForeignKey(i => i.ReaderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
