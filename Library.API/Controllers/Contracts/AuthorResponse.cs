namespace Library.API.Controllers.Contracts
{
    public class AuthorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    }
}
