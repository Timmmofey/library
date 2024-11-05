using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly LibraryDbContext _context;

        public ItemRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Item?> GetByIdAsync(Guid itemId)
        {
            var itemEntity = await _context.Items
                .AsNoTracking()
                .Include(i => i.Authors)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (itemEntity == null) return null;

            var authors = itemEntity.Authors.Select(a => Author.Create(a.Id, a.Name, a.Description).Author).ToList();
            return Item.Create(itemEntity.Id, itemEntity.Title, itemEntity.CategoryId, itemEntity.Publications, authors).Item;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            var itemEntities = await _context.Items
                .AsNoTracking()
                .Include(i => i.Authors)
                .ToListAsync();

            var items = itemEntities.Select(itemEntity =>
            {
                var authors = itemEntity.Authors.Select(a => Author.Create(a.Id, a.Name, a.Description).Author).ToList();
                return Item.Create(itemEntity.Id, itemEntity.Title, itemEntity.CategoryId, itemEntity.Publications, authors).Item;
            }).ToList();

            return items;
        }

        public async Task<Guid> AddAsync(Item item)
        {
            var existingAuthors = await _context.Authors
                .Where(a => item.Authors.Select(author => author.Id).Contains(a.Id))
                .ToListAsync();

            if (existingAuthors.Count != item.Authors.Count)
            {
                throw new ArgumentException("Some authors do not exist.");
            }

            var itemEntity = new ItemEntity
            {
                Id = item.Id,
                Title = item.Title,
                CategoryId = item.CategoryId,
                Publications = item.Publications,
                Authors = existingAuthors
            };

            await _context.Items.AddAsync(itemEntity);
            await _context.SaveChangesAsync();

            return item.Id;
        }

        public async Task<Guid> UpdateAsync(Item item)
        {
            var itemEntity = await _context.Items
                .Include(i => i.Authors)
                .FirstOrDefaultAsync(i => i.Id == item.Id);

            if (itemEntity == null) throw new KeyNotFoundException("Item not found.");

            var existingAuthors = await _context.Authors
                .Where(a => item.Authors.Select(author => author.Id).Contains(a.Id))
                .ToListAsync();

            if (existingAuthors.Count != item.Authors.Count)
            {
                throw new ArgumentException("Some authors do not exist.");
            }

            itemEntity.Title = item.Title;
            itemEntity.CategoryId = item.CategoryId;
            itemEntity.Publications = item.Publications;
            itemEntity.Authors = existingAuthors;

            await _context.SaveChangesAsync();

            return item.Id;
        }

        public async Task<Guid> DeleteAsync(Guid itemId)
        {
            var itemEntity = await _context.Items.FindAsync(itemId);
            if (itemEntity == null) throw new KeyNotFoundException("Item not found.");

            _context.Items.Remove(itemEntity);
            await _context.SaveChangesAsync();

            return itemId;
        }

        public async Task<List<Item>> SearchItemsAsync(string? title, Guid? categoryId, string? publicationDate, List<Guid>? authorIds)
        {
            var query = _context.Items
                .Include(i => i.Authors)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(i => i.Title.Trim().ToLower().Contains(title.Trim().ToLower()));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(publicationDate))
            {
                query = query.Where(i => i.Publications.Trim().ToLower().Contains(publicationDate.Trim().ToLower()));
            }

            if (authorIds != null && authorIds.Count > 0)
            {
                query = query.Where(i => i.Authors.Any(a => authorIds.Contains(a.Id)));
            }

            var itemEntities = await query.AsNoTracking().ToListAsync();

            var items = itemEntities.Select(itemEntity =>
            {
                var authors = itemEntity.Authors.Select(a => Author.Create(a.Id, a.Name, a.Description).Author).ToList();
                return Item.Create(itemEntity.Id, itemEntity.Title, itemEntity.CategoryId, itemEntity.Publications, authors).Item;
            }).ToList();

            return items;
        }


    }
}
