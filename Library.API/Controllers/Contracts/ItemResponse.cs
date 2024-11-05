namespace Library.API.Controllers.Contracts
{
    public class ItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string PublicationDate { get; set; } = string.Empty;
        public List<AuthorResponse> Authors { get; set; } = new List<AuthorResponse>(); 
        public List<ItemCopyResponse> ItemCopies { get; set; } = new List<ItemCopyResponse>(); 
    }
}
