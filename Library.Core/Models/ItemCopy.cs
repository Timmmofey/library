namespace Library.Core.Models
{
    public class ItemCopy
    {
        public const int MAX_INVENTORY_NUMBER_LENGTH = 50;

        private ItemCopy(Guid id, Guid itemId, Guid shelfId, string inventoryNumber, bool loanable, bool loaned, bool lost, DateTime? dateReceived = null, DateTime? dateWithdrawn = null)
        {
            Id = id;
            ItemId = itemId;
            ShelfId = shelfId;
            InventoryNumber = inventoryNumber;
            Loanable = loanable;
            Loaned = loaned;
            Lost = lost;
            Loans = new List<Loan>();
            DateReceived = dateReceived?.Date; // Сохраняем только дату без времени
            DateWithdrawn = dateWithdrawn?.Date; // Сохраняем только дату без времени
        }

        public Guid Id { get; }
        public Guid ItemId { get; }
        public Guid ShelfId { get; }
        public string InventoryNumber { get; } = string.Empty;
        public bool Loanable { get; } = true;

        public DateTime? DateReceived { get; } // Изменено на DateTime?
        public DateTime? DateWithdrawn { get; } // Изменено на DateTime?

        public Item Item { get; }
        public Shelf Shelf { get; }
        public ICollection<Loan> Loans { get; }

        public bool Loaned { get; } = false;
        public bool Lost { get; } = false;

        public static (ItemCopy ItemCopy, string Error) Create(Guid id, Guid itemId, Guid shelfId, string inventoryNumber, bool loanable = true, bool loaned = false, bool lost = false, DateTime? dateReceived = null, DateTime? dateWithdrawn = null)
        {
            var error = string.Empty;

            if (itemId == Guid.Empty)
            {
                error = "ItemId cannot be empty.";
            }

            if (shelfId == Guid.Empty)
            {
                error = "ShelfId cannot be empty.";
            }

            if (string.IsNullOrEmpty(inventoryNumber) || inventoryNumber.Length > MAX_INVENTORY_NUMBER_LENGTH)
            {
                error = "Inventory number cannot be empty or exceed 50 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var itemCopy = new ItemCopy(id, itemId, shelfId, inventoryNumber, loanable, loaned, lost, dateReceived, dateWithdrawn);
            return (itemCopy, error);
        }
    }
}
