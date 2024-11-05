namespace Library.API.Controllers.Contracts
{
    public class LoanUpdateRequest
    {
        public Guid ItemCopyId { get; set; }
        public Guid ReaderId { get; set; }
        public Guid LibrarianId { get; set; }

        // Преобразование дат в UTC без времени
        private DateTime _issueDate;
        public DateTime IssueDate
        {
            get => DateTime.SpecifyKind(_issueDate.Date, DateTimeKind.Utc);  // Вернуть дату в формате UTC
            set => _issueDate = DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);  // Присвоить дату в формате UTC
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => DateTime.SpecifyKind(_dueDate.Date, DateTimeKind.Utc);
            set => _dueDate = DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);
        }

        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get => _returnDate.HasValue ? DateTime.SpecifyKind(_returnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            set => _returnDate = value.HasValue ? DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
        }

        public bool? Lost { get; set; }
    }
}
