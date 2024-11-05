namespace Library.DataAccess.Entities
{
    public class ReadingRoomEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid LibraryId { get; set; }

        public LibraryEntity Library { get; }

        public ICollection<LibrarianEntity> Librarians { get; }

        public ICollection<SectionEntity> Sections { get; }
    }
}
