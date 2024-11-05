namespace Library.Core.Models
{
    public class Librarian
    {
        public Guid Id { get; }
        public string Name { get; }

        public Guid? LibraryId { get; } = null;
        public Guid? ReadingRoomId { get; } = null;
        public string Login { get; }
        public string PasswordHash { get; }
        public Role Role { get; }

        public List<Loan>? Loans { get; }

        private Librarian(Guid id, string name, string login, string passwordHash, Role role, Guid? libraryId = null, Guid? readingRoomId = null, List<Loan>? loans = null)
        {
            Id = id;
            Name = name;
            LibraryId = libraryId;
            ReadingRoomId = readingRoomId;
            Login = login;
            PasswordHash = passwordHash;
            Role = role;
            Loans = loans ?? new List<Loan>();
        }

        public static (Librarian Librarian, string Error) Create(Guid id, string name, string login, string passwordHash, Role role, Guid? libraryId = null, Guid? readingRoomId = null, List<Loan>? loans = null)
        {
            if (string.IsNullOrEmpty(name)) return (null, "Name cannot be empty");
            if (string.IsNullOrEmpty(login)) return (null, "Login cannot be empty");
            if (string.IsNullOrEmpty(passwordHash)) return (null, "PasswordHash cannot be empty");

            return (new Librarian(id, name, login, passwordHash, role, libraryId, readingRoomId, loans), null);
        }
    }
}
