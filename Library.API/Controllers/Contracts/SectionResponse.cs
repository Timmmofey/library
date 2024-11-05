namespace Library.API.Controllers.Contracts
{
    public class SectionResponse
    {
        public Guid Id { get; set; }
        public Guid ReadingRoomId { get; set; }
        public string Name { get; set; }
        public List<ShelveResponse> Shelves { get; set; } 
    }
}

