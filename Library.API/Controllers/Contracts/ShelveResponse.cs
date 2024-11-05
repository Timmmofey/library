namespace Library.API.Controllers.Contracts
{
    public class ShelveResponse
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; } 
        public string Number { get; set; }
    }
}
