namespace Library.Core.DTOs
{
    public class LibrarianWorkReport
    {
        public Guid LibrarianId { get; set; }
        public string LibrarianName { get; set; }
        public int NumberOfServedReaders { get; set; }
    }

}
