namespace Library.Core.Models
{
    public class LibraryModel
    {
        public const int MAX_NAME_LENGTH = 250;
        public const int MAX_ADDRESS_LENGTH = 500;

        private LibraryModel(Guid id, string name, string address, string description,
                             ICollection<ReadingRoom> readingRooms = null,
                             ICollection<Reader> readers = null)
        {
            Id = id;
            Name = name;
            Address = address;
            Description = description;
            ReadingRooms = readingRooms ?? new List<ReadingRoom>(); 
            Readers = readers ?? new List<Reader>();   
            ; 
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Address { get; } = string.Empty;
        public string Description { get; } = string.Empty;

        public ICollection<ReadingRoom> ReadingRooms { get; }
        public ICollection<Reader> Readers { get; }

        public static (LibraryModel Library, string Error) Create(Guid id, string name, string address, string description,
                                                                  ICollection<ReadingRoom> readingRooms = null,
                                                                  ICollection<Reader> readers = null
                                                                  )
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or exceed 250 characters.";
            }

            if (string.IsNullOrEmpty(address) || address.Length > MAX_ADDRESS_LENGTH)
            {
                error = "Address cannot be empty or exceed 500 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var library = new LibraryModel(id, name, address, description, readingRooms, readers);
            return (library, error);
        }
    }
}
