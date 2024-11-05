using Library.Core.Models;
using System.Threading.Tasks;

namespace Library.Core.Abstaction
{
    public interface IItemCopyService
    {
        Task<ItemCopy?> GetByIdAsync(Guid itemCopyId);
        Task<List<ItemCopy>> GetAllAsync();
        Task<Guid> AddAsync(ItemCopy itemCopy);
        Task<Guid> UpdateAsync(ItemCopy itemCopy);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<ItemCopy>> SearchItemCopiesAsync(string? inventoryNumber, Guid? itemId, Guid? shelfId);
        Task<Guid> ReportLoan(Guid itemCopyId);
        Task<Guid> CancelLoan(Guid itemCopyId);
        Task<Guid> ReportLostAsync(Guid itemCopyId);
        Task<Guid> CancelLostAsync(Guid itemCopyId);

        Task<Guid> WithdrawnItemCopy(Guid id);
    }

}
