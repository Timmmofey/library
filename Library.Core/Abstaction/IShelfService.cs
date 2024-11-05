using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IShelfService
    {
        Task<Shelf?> GetShelfByIdAsync(Guid shelfId);
        Task<List<Shelf>> GetAllShelvesAsync();
        Task<Guid> AddShelfAsync(Shelf shelf);
        Task<Guid> UpdateShelfAsync(Shelf shelf);
        Task<Guid> DeleteShelfAsync(Guid shelfId);
        Task<List<Shelf>> SearchShelvesAsync(string? number);
    }
}
