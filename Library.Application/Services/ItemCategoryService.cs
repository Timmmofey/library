using Library.Core.Abstaction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;

        public ItemCategoryService(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        // Получение категории по ID
        public async Task<ItemCategory> GetByIdAsync(Guid id)
        {
            return await _itemCategoryRepository.GetByIdAsync(id);
        }

        // Получение всех категорий
        public async Task<IEnumerable<ItemCategory>> GetAllAsync()
        {
            return await _itemCategoryRepository.GetAllAsync();
        }

        // Создание новой категории
        public async Task<Guid> AddAsync(string name, string description)
        {
            var (category, error) = ItemCategory.Create(Guid.NewGuid(), name, description);
            if (category == null)
            {
                throw new ArgumentException(error);
            }

            return await _itemCategoryRepository.AddAsync(category);
        }

        // Обновление категории
        public async Task UpdateAsync(ItemCategory category)
        {
            var existingCategory = await GetByIdAsync(category.Id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found.");
            }

            await _itemCategoryRepository.UpdateAsync(category);
        }

        // Удаление категории
        public async Task DeleteAsync(Guid id)
        {
            var existingCategory = await GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found.");
            }

            await _itemCategoryRepository.DeleteAsync(id);
        }
    }
}
