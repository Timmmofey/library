using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IShelfRepository
    {
        Task<Shelf?> GetByIdAsync(Guid shelfId);
        Task<List<Shelf>> GetAllAsync();
        Task<Guid> AddAsync(Shelf shelf);
        Task<Guid> UpdateAsync(Shelf shelf);
        Task<Guid> DeleteAsync(Guid shelfId);
        Task<List<Shelf>> SearchShelfsAsync(string? number);
    }
}
