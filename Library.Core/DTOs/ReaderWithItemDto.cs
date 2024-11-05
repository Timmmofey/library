namespace Library.Core.DTOs
{
    public class ReaderWithItemDto
    {
        public Guid ReaderId { get; set; }
        public string ReaderFullName { get; set; }
        public string ItemTitle { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
