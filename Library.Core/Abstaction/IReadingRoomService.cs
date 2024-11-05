using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IReadingRoomService
    {
        Task<ReadingRoom> GetByIdAsync(Guid id);
        Task<List<ReadingRoom>> GetAllAsync();
        Task<Guid> AddAsync(ReadingRoom readingRoom);
        Task<Guid> UpdateAsync(ReadingRoom readingRoom);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<ReadingRoom>> SearchReadingRoomsAsync(string? name);
    }
}
