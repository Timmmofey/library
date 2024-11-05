using Library.Core.DTOs;
using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface ILibrarianRepository
    {
        Task<Librarian> GetByIdAsync(Guid id);
        Task<List<Librarian>> GetAllAsync();
        Task<Guid> AddAsync(Librarian librarian);
        Task<Guid> UpdateAsync(Librarian librarian);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<LibrarianDto1>> SearchLibrariansAsync(string? name, string? libraryName);
        Task<Librarian?> GetByLogin(string login);

        Task<Guid> AddDirector(Librarian librarian);
        Task<Guid> AddLibrarian(Librarian librarian);

        Task<Guid> EditInfo(Guid id, string? name, string? login, string? passwordHash, Guid? libraryId, Guid? ReadingRoomId);
        //Task<bool> LoginExistsAsync(string login);
    }
}
