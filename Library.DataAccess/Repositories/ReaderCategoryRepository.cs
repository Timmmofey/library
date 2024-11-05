using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ReaderCategoryRepository : IReaderCategoryRepository
    {
        private readonly LibraryDbContext _context;

        public ReaderCategoryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ReaderCategory> GetByIdAsync(Guid id)
        {
            var categoryEntity = await _context.ReaderCategories
                .Include(c => c.Readers)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoryEntity == null) return null;

            var category = ReaderCategory.Create(
                categoryEntity.Id,
                categoryEntity.Name,
                categoryEntity.Description
            ).Category;

            return category;
        }

        public async Task<List<ReaderCategory>> GetAllAsync()
        {
            var categoryEntities = await _context.ReaderCategories
                .Include(c => c.Readers)
                .AsNoTracking()
                .ToListAsync();

            var categories = categoryEntities.Select(c => ReaderCategory.Create(
                c.Id,
                c.Name,
                c.Description
            ).Category).ToList();

            return categories;
        }

        public async Task<Guid> AddAsync(ReaderCategory category)
        {
            var categoryEntity = new ReaderCategoryEntity
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            await _context.ReaderCategories.AddAsync(categoryEntity);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<Guid> UpdateAsync(ReaderCategory category)
        {
            var categoryEntity = await _context.ReaderCategories.FindAsync(category.Id);
            if (categoryEntity == null) throw new KeyNotFoundException("Category not found.");

            categoryEntity.Name = category.Name;
            categoryEntity.Description = category.Description;

            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var categoryEntity = await _context.ReaderCategories.FindAsync(id);
            if (categoryEntity == null) throw new KeyNotFoundException("Category not found.");

            _context.ReaderCategories.Remove(categoryEntity);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
