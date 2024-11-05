
namespace Library.API.Controllers.Contracts
{
    public class ReaderResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Guid LibraryId { get; set; }
        public Guid? ReaderCategoryId { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string? EducationalInstitution { get; set; }
        public string? Faculty { get; set; }
        public string? Course { get; set; }
        public string? GroupNumber { get; set; }
        public string? Organization { get; set; }
        public string? ResearchTopic { get; set; }
        public List<LoanResponse> Loans { get; set; } = new List<LoanResponse>();
    }
}

