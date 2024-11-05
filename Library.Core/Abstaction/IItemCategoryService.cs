using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IItemCategoryService
    {
        Task<ItemCategory> GetByIdAsync(Guid id);
        Task<IEnumerable<ItemCategory>> GetAllAsync();
        Task<Guid> AddAsync(string name, string description);
        Task UpdateAsync(ItemCategory category);
        Task DeleteAsync(Guid id);
    }
}
