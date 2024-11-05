using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IReaderCategoryService
    {
        Task<ReaderCategory> GetByIdAsync(Guid id);
        Task<List<ReaderCategory>> GetAllAsync();
        Task<Guid> AddAsync(string name, string description);
        Task<Guid> UpdateAsync(Guid id, string name, string description);
        Task<Guid> DeleteAsync(Guid id);
    }
}
