using Library.Core.Models;

namespace Library.Core.DTOs
{
    public class LibrarianDto1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public Guid? LibraryId { get; set; }
        public Role Role { get; set; }
    }

}
