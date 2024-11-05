using Library.Core.Abstaction;
using Library.Core.Abstraction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Guid> AddAsync(Author author)
        {
            // Дополнительная логика перед добавлением, если нужно
            return await _authorRepository.AddAsync(author);
        }

        public async Task<Guid> UpdateAsync(Author author)
        {
            // Дополнительная логика перед обновлением, если нужно
            return await _authorRepository.UpdateAsync(author);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            // Дополнительная логика перед удалением, если нужно
            return await _authorRepository.DeleteAsync(id);
        }

        public async Task<List<Author>> SearchAuthorsAsync(string? name)
        {
            return await _authorRepository.SearchAuthorsAsync(name);
        }
    }
}
