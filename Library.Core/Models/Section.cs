namespace Library.Core.Models
{
    public class Section
    {
        public const int MAX_NAME_LENGTH = 250;

        private Section(Guid id, Guid readingRoomId, string name, ICollection<Shelf>? shelfs = null)
        {
            Id = id;
            ReadingRoomId = readingRoomId;
            Name = name;
            Shelves = shelfs ?? new List<Shelf>();
        }

        public Guid Id { get; }
        public Guid ReadingRoomId { get; }
        public string Name { get; } = string.Empty;

        public ReadingRoom ReadingRoom { get; }
        public ICollection<Shelf> Shelves { get; }

        public static (Section Section, string Error) Create(Guid id, Guid readingRoomId, string name, ICollection<Shelf>? shelfs = null)
        {
            var error = string.Empty;

            if (readingRoomId == Guid.Empty)
            {
                error = "LibraryId cannot be empty.";
            }

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or exceed 250 characters.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var section = new Section(id, readingRoomId, name, shelfs);
            return (section, error);
        }
    }
}
