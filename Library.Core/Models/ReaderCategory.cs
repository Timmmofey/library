namespace Library.Core.Models
{
    public class ReaderCategory
    {
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_DESCRIPTION_LENGTH = 500;

        private ReaderCategory(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Readers = new List<Reader>();
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public ICollection<Reader> Readers { get; }

        public static (ReaderCategory Category, string Error) Create(Guid id, string name, string description)
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

            var category = new ReaderCategory(id, name, description);
            return (category, error);
        }
    }
}

