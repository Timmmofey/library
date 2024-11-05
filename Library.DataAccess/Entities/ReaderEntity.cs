using Library.Core.Models;

namespace Library.DataAccess.Entities
{
    public class ReaderEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public Guid LibraryId { get; set; }
        public Role Role { get; set; }
        public Guid? ReaderCategoryId { get; set; }

        public string? EducationalInstitution { get; set; }
        public string? Faculty { get; set; }
        public string? Course { get; set; }
        public string? GroupNumber { get; set; }

        public string? Organization { get; set; } 
        public string? ResearchTopic { get; set; }

        public ICollection<LoanEntity>? Loans { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public LibraryEntity Library { get; set; }

        public ReaderCategoryEntity ReaderCategory { get; set; }
    }
}
