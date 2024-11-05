namespace Library.Core.Models
{
    public class Shelf
    {
        public const int MAX_NUMBER_LENGTH = 50;

        private Shelf(Guid id, Guid sectionId, string number, ICollection<ItemCopy>? itemsCopies = null)
        {
            Id = id;
            SectionId = sectionId;
            Number = number;
            ItemCopies = itemsCopies ?? new List<ItemCopy>();
        }

        public Guid Id { get; }
        public Guid SectionId { get; }
        public string Number { get; } = string.Empty;

        public Section Section { get; }
        public ICollection<ItemCopy> ItemCopies { get; }

        public static (Shelf Shelf, string Error) Create(Guid id, Guid sectionId, string number, ICollection<ItemCopy>? itemsCopies = null)
        {
            var error = string.Empty;

            if (sectionId == Guid.Empty)
            {
                error = "SectionId cannot be empty.";
            }

            if (string.IsNullOrEmpty(number) || number.Length > MAX_NUMBER_LENGTH)
            {
                error = "Number cannot be empty or exceed 50 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var shelf = new Shelf(id, sectionId, number, itemsCopies);
            return (shelf, error);
        }
    }
}
