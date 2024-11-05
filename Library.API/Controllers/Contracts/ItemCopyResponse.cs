namespace Library.API.Controllers.Contracts
{
    public class ItemCopyResponse
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid ShelfId { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public bool Loanable { get; set; } = true;
        public bool Loaned { get; set; } = false;

        public bool Lost { get; set; } = false;


        // Изменение типов на DateTime для удобства
        public DateTime? DateReceived { get; set; } // Принять как UTC
        public DateTime? DateWithdrawn { get; set; } // Принять как UTC
    }
}
