namespace Library.Core.Models
{
    public class ReadingRoom
    {
        public const int MAX_NAME_LENGTH = 100;

        private ReadingRoom(Guid id, string name, Guid libraryId, ICollection<Librarian>? librarians = null, ICollection<Section>? sections = null)
        {
            Id = id;
            Name = name;
            LibraryId = libraryId;
            Librarians = librarians ?? new List<Librarian>();
            Sections = sections ?? new List<Section>();
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public Guid LibraryId { get; }
        public LibraryModel Library { get; }
        public ICollection<Librarian> Librarians { get; }
        public ICollection<Section> Sections { get; }

        public static (ReadingRoom ReadingRoom, string Error) Create(Guid id, string name, Guid libraryId, ICollection<Librarian>? librarians = null, ICollection<Section>? sections = null)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or exceed 100 characters.";
            }

            if (libraryId == Guid.Empty)
            {
                error = "LibraryId cannot be empty.";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var readingRoom = new ReadingRoom(id, name, libraryId, librarians, sections);
            return (readingRoom, error);
        }
    }
}

