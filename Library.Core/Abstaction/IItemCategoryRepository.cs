using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> GetByIdAsync(Guid id);

        Task<List<ItemCategory>> GetAllAsync();

        Task<Guid> AddAsync(ItemCategory category);

        Task UpdateAsync(ItemCategory category);

        Task DeleteAsync(Guid id);
    }
}
