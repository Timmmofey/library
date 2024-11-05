namespace Library.Core.DTOs
{
    public class LoanDto
    {
        public Guid Id { get; set; }
        public Guid ItemCopyId { get; set; }
        public Guid ReaderId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
