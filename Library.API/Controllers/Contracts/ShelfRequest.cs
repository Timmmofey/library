namespace Library.API.Controllers.Contracts
{
    public class ShelfRequest
    {
        public Guid SectionId { get; set; } 
        public string Number { get; set; } = string.Empty;
    }
}
