using Library.Core.Abstaction;
using Library.Core.Abstraction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class LibrariesService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibrariesService(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryModel> GetLibraryByIdAsync(Guid id)
        {
            return await _libraryRepository.GetByIdAsync(id);
        }

        public async Task<List<LibraryModel>> GetAllLibrariesAsync()
        {
            return await _libraryRepository.GetAllAsync();
        }

        public async Task<Guid> AddLibraryAsync(LibraryModel libraryModel)
        {
            return await _libraryRepository.AddAsync(libraryModel);
        }

        public async Task<Guid> UpdateLibraryAsync(LibraryModel libraryModel)
        {
            return await _libraryRepository.UpdateAsync(libraryModel);
        }

        public async Task<Guid> DeleteLibraryAsync(Guid id)
        {
            return await _libraryRepository.DeleteAsync(id);
        }

        public async Task<List<LibraryModel>> SearchLibrariesAsync(string? name, string? address, string? description)
        {
            return await _libraryRepository.SearchLibrariesAsync(name, address, description);
        }

    }
}
