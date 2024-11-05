namespace Library.DataAccess.Entities
{
    public class LibraryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public ICollection<ReadingRoomEntity> ReadingRooms { get; }
        public ICollection<ReaderEntity> Readers { get; set; }
    }
}
