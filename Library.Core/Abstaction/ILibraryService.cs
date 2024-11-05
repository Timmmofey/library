using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface ILibraryService
    {
        Task<List<LibraryModel>> GetAllLibrariesAsync();
        Task<LibraryModel> GetLibraryByIdAsync(Guid id);
        Task<Guid> AddLibraryAsync(LibraryModel library);
        Task<Guid> UpdateLibraryAsync(LibraryModel library);
        Task<Guid> DeleteLibraryAsync(Guid id);
        Task<List<LibraryModel>> SearchLibrariesAsync(string? name, string? address, string? description);
    }
}
