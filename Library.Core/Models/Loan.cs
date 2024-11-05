namespace Library.Core.Models
{
    public class Loan
    {
        private Loan(Guid id, Guid itemCopyId, Guid readerId, Guid librarianId, DateTime issueDate, DateTime dueDate, DateTime? returnDate, bool? lost)
        {
            Id = id;
            ItemCopyId = itemCopyId;
            ReaderId = readerId;
            LibrarianId = librarianId;
            IssueDate = DateTime.SpecifyKind(issueDate.Date, DateTimeKind.Utc);
            DueDate = DateTime.SpecifyKind(dueDate.Date, DateTimeKind.Utc);
            ReturnDate = returnDate.HasValue ? DateTime.SpecifyKind(returnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            Lost = lost;
        }

        public Guid Id { get; }
        public Guid ItemCopyId { get; }
        public Guid ReaderId { get; }
        public Guid LibrarianId { get; }
        public DateTime IssueDate { get; }
        public DateTime DueDate { get; }
        public DateTime? ReturnDate { get; }
        public bool? Lost { get; }

        public ItemCopy ItemCopy { get; }
        public Reader Reader { get; }
        public Librarian Librarian { get; }

        public static (Loan Loan, string Error) Create(Guid id, Guid itemCopyId, Guid readerId, Guid librarianId, DateTime issueDate, DateTime dueDate, DateTime? returnDate = null, bool? lost = false)
        {
            var error = string.Empty;

            issueDate = DateTime.SpecifyKind(issueDate.Date, DateTimeKind.Utc);
            dueDate = DateTime.SpecifyKind(dueDate.Date, DateTimeKind.Utc);
            returnDate = returnDate.HasValue ? DateTime.SpecifyKind(returnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;

            if (itemCopyId == Guid.Empty)
            {
                error = "ItemCopyId cannot be empty.";
            }

            if (readerId == Guid.Empty)
            {
                error = "ReaderId cannot be empty.";
            }

            if (librarianId == Guid.Empty)
            {
                error = "LibrarianId cannot be empty.";
            }

            if (issueDate > DateTime.UtcNow)
            {
                error = "Issue date cannot be in the future.";
            }

            if (dueDate <= issueDate)
            {
                error = "Due date must be after the issue date.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var loan = new Loan(id, itemCopyId, readerId, librarianId, issueDate, dueDate, returnDate, lost);
            return (loan, error);
        }

    }
}
