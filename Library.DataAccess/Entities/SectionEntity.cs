namespace Library.DataAccess.Entities
{
    public class SectionEntity
    {
        public Guid Id { get; set; }
        public Guid ReadingRoomId { get; set; }
        public string Name { get; set; }

        public ReadingRoomEntity ReadingRoom { get; }
        public ICollection<ShelfEntity> Shelves { get; set; }
    }
}
