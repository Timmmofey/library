namespace Library.API.Controllers.Contracts
{
    public class LibraryQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<SectionResponse> Sections { get; set; } = new List<SectionResponse>();
        public List<ReaderResponse> Readers { get; set; } = new List<ReaderResponse>();
        public List<LibrarianResponse> Librarians { get; set; } = new List<LibrarianResponse>();
    }
}

