namespace Library.Core.Models
{
    public class Item
    {
        public const int MAX_TITLE_LENGTH = 250;

        private Item(Guid id, string title, Guid categoryId, string publications, ICollection<Author> authors)
        {
            Id = id;
            Title = title;
            CategoryId = categoryId;
            Publications = publications;
            Authors = authors ?? new List<Author>();
            ItemCopies = new List<ItemCopy>();
        }

        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public Guid CategoryId { get; }

        public string Publications { get; }
        public ICollection<Author> Authors { get; } = new List<Author>();
        public ItemCategory Category { get; }
        public ICollection<ItemCopy> ItemCopies { get; }

        public static (Item Item, string Error) Create(Guid id, string title, Guid categoryId, string publications, ICollection<Author> authors)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error = "Title cannot be empty or exceed 250 characters.";
            }

            if (categoryId == Guid.Empty)
            {
                error = "CategoryId cannot be empty.";
            }

            if (publications == null )
            {
                error = "Publications cannot be empty.";
            }

            if (authors == null || authors.Count == 0)
            {
                error = "At least one author must be specified.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var item = new Item(id, title, categoryId, publications, authors);
            return (item, error);
        }
    }
}


