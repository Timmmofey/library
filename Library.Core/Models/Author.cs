namespace Library.Core.Models
{
    public class Author
    {
        public const int MAX_NAME_LENGTH = 150;
        public const int MAX_DESCRIPTION_LENGTH = 1000;

        private Author(Guid id, string name, string description, ICollection<Item>? items = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Items = items ?? new List<Item>();
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;

        public ICollection<Item> Items { get; }

        public static (Author Author, string Error) Create(Guid id, string name, string description, ICollection<Item>? items = null)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or exceed 150 characters.";
            }

            if (!string.IsNullOrEmpty(description) && description.Length > MAX_DESCRIPTION_LENGTH)
            {
                error = "Description cannot exceed 1000 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var author = new Author(id, name, description, items);
            return (author, error);
        }
    }
}

