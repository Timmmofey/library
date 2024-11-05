namespace Library.DataAccess.Entities
{
    public class ItemCopyEntity
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid ShelfId { get; set; }
        public string InventoryNumber { get; set; }
        public bool Loanable { get; set; } = true;

        public bool Loaned { get; set; } = false;
        public bool Lost { get; set; } = false;

        public DateTime? DateReceived { get; set; } // Изменено на DateTime?
        public DateTime? DateWithdrawn { get; set; } // Изменено на DateTime?

        public ItemEntity Item { get; set; }
        public ShelfEntity Shelf { get; set; }
        public ICollection<LoanEntity> Loans { get; set; }
    }
}
