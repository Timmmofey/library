namespace Library.API.Controllers.Contracts
{
    public class SectionRequest
    {
        public Guid ReadingRoomId { get; set; }
        public string Name { get; set; }
    }
}
