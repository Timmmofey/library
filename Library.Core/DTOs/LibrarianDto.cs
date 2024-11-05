using Library.Core.Models;

namespace Library.Core.DTOs
{
    public class LibrarianDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ReadingRoomName { get; set; }
        public string? LibraryName { get; set; }
        public Role Role { get; set; } // Новое поле
        public List<LoanDto>? Loans { get; set; } = new List<LoanDto>();
    }
}
