using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<LoanEntity>
    {
        public void Configure(EntityTypeBuilder<LoanEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.ItemCopy)
                .WithMany(i => i.Loans);

            builder
                .HasOne(i => i.Librarian)
                .WithMany(i => i.Loans);

            builder
                .HasOne(i => i.Reader)
                .WithMany(i => i.Loans);
        }
    }
