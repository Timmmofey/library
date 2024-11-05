
namespace Library.API.Controllers.Contracts
{
    public class ItemSearchRequest
    {
        public string? Title { get; set; }
        public Guid? CategoryId { get; set; }
        public string? PublicationDate { get; set; }
        public List<Guid>? AuthorIds { get; set; }
    }
}
