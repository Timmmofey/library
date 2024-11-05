using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IItemService
    {
        Task<Item?> GetByIdAsync(Guid id);
        Task<List<Item>> GetAllAsync();
        Task<Guid> AddAsync(Item item);
        Task<Guid> UpdateAsync(Item item);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<Item>> SearchItemsAsync(string? title, Guid? categoryId, string? publicationDate, List<Guid>? authorIds);
    }
}
