namespace Library.Core.DTOs
{
    public class ReaderDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid LibraryId { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
}
