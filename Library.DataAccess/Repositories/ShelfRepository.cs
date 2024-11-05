using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly LibraryDbContext _context;

        public ShelfRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Shelf?> GetByIdAsync(Guid shelfId)
        {
            var shelfEntity = await _context.Shelfs
                .AsNoTracking()
                .Include(s => s.ItemCopies)
                .FirstOrDefaultAsync(s => s.Id == shelfId);

            if (shelfEntity == null)
            {
                return null;
            }

            var shelf = Shelf.Create(
                shelfEntity.Id,
                shelfEntity.SectionId,
                shelfEntity.Number,
                shelfEntity.ItemCopies.Select(i => ItemCopy.Create(
                    i.Id,
                    i.ItemId,
                    i.ShelfId,
                    i.InventoryNumber,
                    i.Loanable,
                    i.Loaned,
                    i.Lost,
                    i.DateReceived,
                    i.DateWithdrawn
                    ).ItemCopy).ToList()
            ).Shelf;

            return shelf;
        }

        public async Task<List<Shelf>> GetAllAsync()
        {
            var shelfEntities = await _context.Shelfs
                .AsNoTracking()
                .Include(s => s.ItemCopies)
                .ToListAsync();

            var Shelfs = shelfEntities.Select(shelfEntity =>
            {
                var shelf = Shelf.Create(
                    shelfEntity.Id,
                    shelfEntity.SectionId,
                    shelfEntity.Number,
                    shelfEntity.ItemCopies.Select(i => ItemCopy.Create(
                    i.Id,
                    i.ItemId,
                    i.ShelfId,
                    i.InventoryNumber,
                    i.Loanable,
                    i.Loaned,
                    i.Lost,
                    i.DateReceived,
                    i.DateWithdrawn
                    ).ItemCopy).ToList()
            ).Shelf;
          
                return shelf;
            }).ToList();

            return Shelfs;
        }

        public async Task<Guid> AddAsync(Shelf shelf)
        {
            var shelfEntity = new ShelfEntity
            {
                Id = shelf.Id,
                SectionId = shelf.SectionId,
                Number = shelf.Number,
            };

            await _context.Shelfs.AddAsync(shelfEntity);
            await _context.SaveChangesAsync();

            return shelf.Id;
        }

        public async Task<Guid> UpdateAsync(Shelf shelf)
        {
            var shelfEntity = await _context.Shelfs.FindAsync(shelf.Id);
            if (shelfEntity == null) throw new KeyNotFoundException("Shelf not found.");

            shelfEntity.Number = shelf.Number;
            shelfEntity.SectionId = shelf.SectionId;

            await _context.SaveChangesAsync();

            return shelf.Id;
        }

        public async Task<Guid> DeleteAsync(Guid shelfId)
        {
            var shelfEntity = await _context.Shelfs.FindAsync(shelfId);
            if (shelfEntity == null) throw new KeyNotFoundException("Shelf not found.");

            _context.Shelfs.Remove(shelfEntity);
            await _context.SaveChangesAsync();

            return shelfId;
        }

        public async Task<List<Shelf>> SearchShelfsAsync(string? number)
        {
            var query = _context.Shelfs
                .AsNoTracking()
                .Include(s => s.ItemCopies)
                .AsQueryable();

            if (!string.IsNullOrEmpty(number))
            {
                query = query.Where(s => s.Number.Trim().ToLower().Contains(number.Trim().ToLower()));
            }

            var shelfEntities = await query.ToListAsync();

            var Shelfs = shelfEntities.Select(shelfEntity =>
            {
                var shelf = Shelf.Create(
                    shelfEntity.Id,
                    shelfEntity.SectionId,
                    shelfEntity.Number,
                    shelfEntity.ItemCopies.Select(i => ItemCopy.Create(
                    i.Id,
                    i.ItemId,
                    i.ShelfId,
                    i.InventoryNumber,
                    i.Loanable,
                    i.Loaned,
                    i.Lost,
                    i.DateReceived,
                    i.DateWithdrawn
                    ).ItemCopy).ToList()
                ).Shelf;

                return shelf;
            }).ToList();

            return Shelfs;
        }
    }
}
