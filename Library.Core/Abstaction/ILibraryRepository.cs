using Library.Core.Models;

namespace Library.Core.Abstraction
{
    public interface ILibraryRepository
    {
        Task<LibraryModel> GetByIdAsync(Guid id);
        Task<List<LibraryModel>> GetAllAsync();
        Task<Guid> AddAsync(LibraryModel libraryModel);
        Task<Guid> UpdateAsync(LibraryModel libraryModel);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<LibraryModel>> SearchLibrariesAsync(string? name, string? address, string? description);

        Task<List<(Guid Id, string Name)>> GetLibraryIdsAndNamesAsync();
    }
}
