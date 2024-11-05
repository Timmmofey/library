namespace Library.Core.DTOs
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public Guid ReadingRoomId { get; set; }
        public string Name { get; set; }

        public ICollection<ShelfDto>? Shelves { get; set; }
    }

}
