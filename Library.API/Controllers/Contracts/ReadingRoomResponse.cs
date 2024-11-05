namespace Library.API.Controllers.Contracts
{
    public class ReadingRoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LibraryId { get; set; }

        public List<LibrarianResponse> Librarians { get; set; }
        public List<SectionResponse> Sections { get; set; }
    }
}
