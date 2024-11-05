using Library.Core.Models;

namespace Library.Core.Abstraction
{
    public interface IAuthorService
    {
        Task<Author> GetByIdAsync(Guid id);
        Task<List<Author>> GetAllAsync();
        Task<Guid> AddAsync(Author author);
        Task<Guid> UpdateAsync(Author author);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<Author>> SearchAuthorsAsync(string? name);
    }
}
