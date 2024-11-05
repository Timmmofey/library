using Library.Core.Models;

namespace Library.DataAccess.Entities
{
    public class LibrarianEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LibraryId { get; set; } = null;
        public Guid? ReadingRoomId { get; set; } = null;
        public ReadingRoomEntity ReadingRoom { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<LoanEntity>? Loans { get; set; }

        // Новое поле для роли
        public Role Role { get; set; }
    }
}
