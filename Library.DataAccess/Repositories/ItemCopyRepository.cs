using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Repositories
{
    public class ItemCopyRepository : IItemCopyRepository
    {
        private readonly LibraryDbContext _context;

        public ItemCopyRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCopy?> GetByIdAsync(Guid itemCopyId)
        {
            var itemCopyEntity = await _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item)
                .Include(ic => ic.Shelf)
                .FirstOrDefaultAsync(ic => ic.Id == itemCopyId);

            if (itemCopyEntity == null)
            {
                return null;
            }

            var itemCopy = ItemCopy.Create(
                itemCopyEntity.Id,
                itemCopyEntity.ItemId,
                itemCopyEntity.ShelfId,
                itemCopyEntity.InventoryNumber,
                itemCopyEntity.Loanable,
                itemCopyEntity.Loaned,
                itemCopyEntity.Lost,
                itemCopyEntity.DateReceived?.Date, 
                itemCopyEntity.DateWithdrawn?.Date 
            ).ItemCopy;

            return itemCopy;
        }

        public async Task<List<ItemCopy>> GetAllAsync()
        {
            var itemCopyEntities = await _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item)
                .Include(ic => ic.Shelf)
                .ToListAsync();

            var itemCopies = new List<ItemCopy>();

            foreach (var itemCopyEntity in itemCopyEntities)
            {
                var itemCopy = ItemCopy.Create(
                    itemCopyEntity.Id,
                    itemCopyEntity.ItemId,
                    itemCopyEntity.ShelfId,
                    itemCopyEntity.InventoryNumber,
                    itemCopyEntity.Loanable,
                    itemCopyEntity.Loaned,
                    itemCopyEntity.Lost,
                    itemCopyEntity.DateReceived?.Date, 
                    itemCopyEntity.DateWithdrawn?.Date 
                ).ItemCopy;

                itemCopies.Add(itemCopy);
            }

            return itemCopies;
        }

        public async Task<Guid> AddAsync(ItemCopy itemCopy)
        {
            var itemCopyEntity = new ItemCopyEntity
            {
                Id = itemCopy.Id,
                ItemId = itemCopy.ItemId,
                ShelfId = itemCopy.ShelfId,
                InventoryNumber = itemCopy.InventoryNumber,
                Loanable = itemCopy.Loanable,
                DateReceived = itemCopy.DateReceived.HasValue ? DateTime.SpecifyKind(itemCopy.DateReceived.Value.Date, DateTimeKind.Utc) : (DateTime?)null,
                DateWithdrawn = itemCopy.DateWithdrawn.HasValue ? DateTime.SpecifyKind(itemCopy.DateWithdrawn.Value.Date, DateTimeKind.Utc) : (DateTime?)null
            };

            await _context.ItemCopies.AddAsync(itemCopyEntity);
            await _context.SaveChangesAsync();

            return itemCopy.Id;
        }

        public async Task<Guid> UpdateAsync(ItemCopy itemCopy)
        {
            var itemCopyEntity = await _context.ItemCopies.FindAsync(itemCopy.Id);
            if (itemCopyEntity == null) throw new KeyNotFoundException("ItemCopy not found.");

            itemCopyEntity.ItemId = itemCopy.ItemId;
            itemCopyEntity.ShelfId = itemCopy.ShelfId;
            itemCopyEntity.InventoryNumber = itemCopy.InventoryNumber;
            itemCopyEntity.Loanable = itemCopy.Loanable;
            itemCopyEntity.DateReceived = itemCopy.DateReceived.HasValue ? DateTime.SpecifyKind(itemCopy.DateReceived.Value.Date, DateTimeKind.Utc) : (DateTime?)null; 
            itemCopyEntity.DateWithdrawn = itemCopy.DateWithdrawn.HasValue ? DateTime.SpecifyKind(itemCopy.DateWithdrawn.Value.Date, DateTimeKind.Utc) : (DateTime?)null; 

            await _context.SaveChangesAsync();

            return itemCopy.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var itemCopyEntity = await _context.ItemCopies.FindAsync(id);
            if (itemCopyEntity == null) throw new KeyNotFoundException("ItemCopy not found.");

            _context.ItemCopies.Remove(itemCopyEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<List<ItemCopy>> SearchItemCopiesAsync(string? inventoryNumber, Guid? itemId, Guid? shelfId)
        {
            var query = _context.ItemCopies.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(inventoryNumber))
            {
                query = query.Where(ic => ic.InventoryNumber.Contains(inventoryNumber));
            }

            if (itemId.HasValue)
            {
                query = query.Where(ic => ic.ItemId == itemId.Value);
            }

            if (shelfId.HasValue)
            {
                query = query.Where(ic => ic.ShelfId == shelfId.Value);
            }

            var itemCopyEntities = await query.ToListAsync();

            var itemCopies = new List<ItemCopy>();

            foreach (var itemCopyEntity in itemCopyEntities)
            {
                var itemCopy = ItemCopy.Create(
                    itemCopyEntity.Id,
                    itemCopyEntity.ItemId,
                    itemCopyEntity.ShelfId,
                    itemCopyEntity.InventoryNumber,
                    itemCopyEntity.Loanable,
                    itemCopyEntity.Loaned,
                    itemCopyEntity.Lost,
                    itemCopyEntity.DateReceived?.Date, 
                    itemCopyEntity.DateWithdrawn?.Date  
                ).ItemCopy;

                itemCopies.Add(itemCopy);
            }

            return itemCopies;
        }

        public async Task<Guid> ReportLoan(Guid itemCopyId)
        {
            var loanEntity = await _context.ItemCopies.FindAsync(itemCopyId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.Loaned = true;

            await _context.SaveChangesAsync();

            return itemCopyId;
        }

        public async Task<Guid> CancelLoan(Guid itemCopyId)
        {
            var loanEntity = await _context.ItemCopies.FindAsync(itemCopyId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.Loaned = false;

            await _context.SaveChangesAsync();

            return itemCopyId;
        }

        public async Task<Guid> ReportLostAsync(Guid itemCopyId)
        {
            var loanEntity = await _context.ItemCopies.FindAsync(itemCopyId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.Lost = true;

            await _context.SaveChangesAsync();

            return itemCopyId;
        }

        public async Task<Guid> CancelLostAsync(Guid itemCopyId)
        {
            var loanEntity = await _context.ItemCopies.FindAsync(itemCopyId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.Lost = false; 

            await _context.SaveChangesAsync();

            return itemCopyId;
        }

        public async Task<List<ItemCopy>> GetLoanedItemsByShelfAsync7(Guid shelfId)
        {
            var loanedItems = await _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item) // Включаем информацию о произведении
                .Include(ic => ic.Shelf) // Включаем информацию о полке
                .Where(ic => ic.ShelfId == shelfId && ic.Loaned) // Фильтруем по идентификатору полки и статусу Loaned
                .ToListAsync();

            var itemCopies = new List<ItemCopy>();

            foreach (var itemCopyEntity in loanedItems)
            {
                var itemCopy = ItemCopy.Create(
                    itemCopyEntity.Id,
                    itemCopyEntity.ItemId,
                    itemCopyEntity.ShelfId,
                    itemCopyEntity.InventoryNumber,
                    itemCopyEntity.Loanable,
                    itemCopyEntity.Loaned,
                    itemCopyEntity.Lost,
                    itemCopyEntity.DateReceived?.Date,
                    itemCopyEntity.DateWithdrawn?.Date
                ).ItemCopy;

                itemCopies.Add(itemCopy);
            }

            return itemCopies;
        }

        public async Task<Guid> WithdrawnItemCopy(Guid id)
        {
            var itemCopyEnity = await _context.ItemCopies.FindAsync(id);
            if (itemCopyEnity == null) throw new KeyNotFoundException("ItemCopy not found.");
        
            itemCopyEnity.DateWithdrawn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return id;
        }
    }
}
