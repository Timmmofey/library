using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Configurations;

public class ReadingRoomConfiguration : IEntityTypeConfiguration<ReadingRoomEntity> {
    public void Configure(EntityTypeBuilder<ReadingRoomEntity> builder)
    {
        builder.HasKey(i => i.Id);

        builder
            .HasOne(i => i.Library)
            .WithMany(i => i.ReadingRooms);

        builder
            .HasMany(i => i.Librarians)
            .WithOne(i => i.ReadingRoom)
            .HasForeignKey(i => i.ReadingRoomId)
            .OnDelete(DeleteBehavior.SetNull);


        builder
            .HasMany(i => i.Sections)
            .WithOne(i => i.ReadingRoom)
            .HasForeignKey(i => i.ReadingRoomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
    
