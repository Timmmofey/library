namespace Library.API.Controllers.Contracts
{
    public class ShelveRequest
    {
        public Guid SectionId { get; set; }
        public string Number { get; set; }
    }
}
