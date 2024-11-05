namespace Library.Core.Models
{
    public class Reader
    {
        public const int MAX_FULL_NAME_LENGTH = 150;

        private Reader(Guid id,
                       string email,
                       string passwordHash,
                       string fullName,
                       Guid libraryId,
                       Guid? readerCategoryId = null,
                       ICollection<Loan>? loans = null,
                       DateTime? subscriptionEndDate = null,
                       string? educationalInstitution = null,
                       string? faculty = null,
                       string? course = null,
                       string? groupNumber = null,
                       string? organization = null,
                       string? researchTopic = null)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            ReaderCategoryId = readerCategoryId;
            LibraryId = libraryId;
            Role = Role.Reader;
            Loans = loans ?? new List<Loan>();
            SubscriptionEndDate = subscriptionEndDate;
            EducationalInstitution = educationalInstitution;
            Faculty = faculty;
            Course = course;
            GroupNumber = groupNumber;
            Organization = organization;
            ResearchTopic = researchTopic;
        }

        public Guid Id { get; }

        public string Email { get; }

        public string PasswordHash { get; }
        public string FullName { get; }
        public Guid LibraryId { get; }
        public Role Role { get; }
        public Guid? ReaderCategoryId { get; }
        public string? EducationalInstitution { get; }
        public string? Faculty { get; }
        public string? Course { get; }
        public string? GroupNumber { get; }
        public string? Organization { get; }
        public string? ResearchTopic { get; }
        public ICollection<Loan>? Loans { get; }

        public DateTime? SubscriptionEndDate { get; private set; }

        public LibraryModel Library { get; }
        public ReaderCategory ReaderCategory { get; }

        public void ExtendSubscription(int days)
        {
            if (SubscriptionEndDate.HasValue && SubscriptionEndDate.Value > DateTime.UtcNow)
            {
                SubscriptionEndDate = SubscriptionEndDate.Value.AddDays(days);
            }
            else
            {
                SubscriptionEndDate = DateTime.UtcNow.AddDays(days);
            }
        }

        public static (Reader Reader, string Error) Create(
            Guid id,
            string email,
            string passwordHash,
            string fullName,
            Guid libraryId,
            Guid? readerCategoryId = null,
            ICollection<Loan> loans = null,
            DateTime? subscriptionEndDate = null,
            string? educationalInstitution = null,
            string? faculty = null,
            string? course = null,
            string? groupNumber = null,
            string? organization = null,
            string? researchTopic = null)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(fullName) || fullName.Length > MAX_FULL_NAME_LENGTH)
            {
                error = "Full name cannot be empty or exceed 150 characters.";
            }

            if (readerCategoryId == Guid.Empty)
            {
                error = "ReaderCategoryId cannot be empty.";
            }

            if (libraryId == Guid.Empty)
            {
                error = "LibraryId cannot be empty.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var reader = new Reader(
                id,
                email,
                passwordHash,
                fullName,
                libraryId,
                readerCategoryId,
                loans,
                subscriptionEndDate,
                educationalInstitution,
                faculty,
                course,
                groupNumber,
                organization,
                researchTopic
            );

            return (reader, error);
        }

    }
}