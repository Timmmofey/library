using Library.Core.Abstaction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<Guid> AddAsync(Item item)
        {
            var (newItem, error) = Item.Create(
                item.Id,
                item.Title,
                item.CategoryId,
                item.Publications,
                item.Authors
            );

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }

            return await _itemRepository.AddAsync(newItem);
        }

        public async Task<Guid> UpdateAsync(Item item)
        {
            var (updatedItem, error) = Item.Create(
                item.Id,
                item.Title,
                item.CategoryId,
                item.Publications,
                item.Authors
            );

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }

            return await _itemRepository.UpdateAsync(updatedItem);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _itemRepository.DeleteAsync(id);
        }

        public async Task<List<Item>> SearchItemsAsync(string? title, Guid? categoryId, string? publicationDate, List<Guid>? authorIds)
        {
            return await _itemRepository.SearchItemsAsync(title, categoryId, publicationDate, authorIds);
        }
    }
}
