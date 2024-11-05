using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IReaderCategoryRepository
    {
        Task<ReaderCategory> GetByIdAsync(Guid id);
        Task<List<ReaderCategory>> GetAllAsync();
        Task<Guid> AddAsync(ReaderCategory category);
        Task<Guid> UpdateAsync(ReaderCategory category);
        Task<Guid> DeleteAsync(Guid id);
    }
}
