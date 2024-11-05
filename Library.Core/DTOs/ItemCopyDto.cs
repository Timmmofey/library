namespace Library.Core.DTOs
{
    public class ItemCopyDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid ShelfId { get; set; }
        public string InventoryNumber { get; set; }
        public bool Loanable { get; set; } = true;
        public DateTime? DateReceived { get; set; } 
        public DateTime? DateWithdrawn { get; set; } 

        public ICollection<LoanDto>? Loans { get; set; }
    }
}
