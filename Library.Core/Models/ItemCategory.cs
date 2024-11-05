namespace Library.Core.Models
{
    public class ItemCategory
    {
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_DESCRIPTION_LENGTH = 500;

        private ItemCategory(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Items = new List<Item>();
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public ICollection<Item> Items { get; }

        public static (ItemCategory Category, string Error) Create(Guid id, string name, string description)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or exceed 100 characters.";
            }

            if (!string.IsNullOrEmpty(description) && description.Length > MAX_DESCRIPTION_LENGTH)
            {
                error = "Description cannot exceed 500 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var category = new ItemCategory(id, name, description);
            return (category, error);
        }
    }
}

