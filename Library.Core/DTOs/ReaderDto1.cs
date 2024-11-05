namespace Library.Core.DTOs
{
    public class ReaderDto1
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? LibraryName { get; set; }
        public Guid? ReaderCategoryId { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string? EducationalInstitution { get; set; }
        public string? Faculty { get; set; }
        public string? Course { get; set; }
        public string? GroupNumber { get; set; }
        public string? Organization { get; set; }
        public string? ResearchTopic { get; set; }
    }
}
