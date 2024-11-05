namespace Library.DataAccess.Entities
{
    public class LoanEntity
    {
        public Guid Id { get; set; }
        public Guid ItemCopyId { get; set; }
        public Guid ReaderId { get; set; }
        public Guid LibrarianId { get; set; }  
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public ItemCopyEntity ItemCopy { get; set; }
        public ReaderEntity Reader { get; set; }
        public LibrarianEntity Librarian { get; set; }
        public bool? Lost { get; set; }
    }
}
