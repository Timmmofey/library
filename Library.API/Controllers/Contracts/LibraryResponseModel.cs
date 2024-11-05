using Library.API.Controllers.Contracts;

namespace Library.Controllers.Contracts
{
    public record LibraryResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public List<ReadingRoomResponse> ReadingRooms { get; set; }
        public List<ReaderResponse> Readers { get; set; }
    }

    
}
