namespace Library.API.Controllers.Contracts
{
    public class ItemCategoryResponse
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
    }
}
