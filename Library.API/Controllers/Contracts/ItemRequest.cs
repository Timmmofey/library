
namespace Library.API.Controllers.Contracts
{
    public class ItemRequest
    {
        public string Title { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string PublicationDate { get; set; } = string.Empty;
        public List<Guid> AuthorIds { get; set; } = new List<Guid>(); 
    }
}
