namespace Library.API.Controllers.Contracts
{
    public class ShelfResponse
    {
        public Guid Id { get; set; } 
        public Guid SectionId { get; set; } 
        public string Number { get; set; } = string.Empty;
        public List<ItemCopyResponse> ItemCopies { get; set; } = new List<ItemCopyResponse>();

    }
}
