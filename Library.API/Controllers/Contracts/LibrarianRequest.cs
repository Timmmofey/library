using Library.Core.Models;

namespace Library.API.Controllers.Contracts
{
    public class LibrarianRequest
    {
        public string Name { get; set; } = string.Empty;

        public Guid ReadingRoomId { get; set; }

        public string Login { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Добавление роли, если это необходимо для создания библиотекаря
        public Role Role { get; set; } = Role.Librarian;
    }
}
