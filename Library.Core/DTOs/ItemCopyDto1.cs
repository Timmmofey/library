namespace Library.Core.DTOs
{
    public class ItemCopyDto1
    {
        public Guid ItemCopyId { get; set; }
        public Guid ItemId { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; } = new List<string>();
        public string InventoryNumber { get; set; }
        public bool Loanable { get; set; }
        public bool Loaned { get; set; }
        public bool Lost { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateWithdrawn { get; set; }
        public string Publications { get; set; }  // Добавлено поле для публикаций
    }

}
