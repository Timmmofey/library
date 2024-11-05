using Library.Core.Models;

namespace Library.API.Controllers.Contracts
{
    public class LibrarianDirectorRequest
    {
        public string Name { get; set; } = string.Empty;

        public Guid LibraryId { get; set; }

        public string Login { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Добавление роли, если это необходимо для создания библиотекаря
        //public string Role { get; set; }
    }
}
