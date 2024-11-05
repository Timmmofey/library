using Library.Core.Models;

namespace Library.API.Controllers.Contracts
{
    public class LibrarianResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? LibraryId { get; set; } = null;
        public Guid? ReadingRoomId { get; set; } = null;

        public Role Role { get; set; } = Role.Librarian;

        public List<LoanResponse> Loans { get; set; } = new();

        // Дополнительное поле, если нужно возвращать название читального зала
        //public string? ReadingRoomName { get; set; }
    }
}
