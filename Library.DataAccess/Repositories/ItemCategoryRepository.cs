using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly LibraryDbContext _context;

        public ItemCategoryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCategory> GetByIdAsync(Guid id)
        {
            var categoryEntity = await _context.ItemCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoryEntity == null) return null;

            var category = ItemCategory.Create(
                categoryEntity.Id,
                categoryEntity.Name,
                categoryEntity.Description
            ).Category;

            return category;
        }

        public async Task<List<ItemCategory>> GetAllAsync()
        {
            var categoryEntities = await _context.ItemCategories
                .AsNoTracking()
                .ToListAsync();

            var categories = categoryEntities.Select(c => ItemCategory.Create(
                c.Id,
                c.Name,
                c.Description
            ).Category).ToList();

            return categories;
        }

        public async Task<Guid> AddAsync(ItemCategory category)
        {
            var categoryEntity = new ItemCategoryEntity
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            await _context.ItemCategories.AddAsync(categoryEntity);

            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateAsync(ItemCategory category)
        {
            var existingEntity = await _context.ItemCategories
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingEntity != null)
            {
                existingEntity.Name = category.Name;
                existingEntity.Description = category.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var categoryEntity = await _context.ItemCategories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoryEntity != null)
            {
                _context.ItemCategories.Remove(categoryEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
