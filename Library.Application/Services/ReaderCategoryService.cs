using Library.Core.Abstaction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class ReaderCategoryService : IReaderCategoryService
    {
        private readonly IReaderCategoryRepository _readerCategoryRepository;

        public ReaderCategoryService(IReaderCategoryRepository readerCategoryRepository)
        {
            _readerCategoryRepository = readerCategoryRepository;
        }

        // Получение категории читателя по ID
        public async Task<ReaderCategory> GetByIdAsync(Guid id)
        {
            return await _readerCategoryRepository.GetByIdAsync(id);
        }

        // Получение всех категорий читателей
        public async Task<List<ReaderCategory>> GetAllAsync()
        {
            return await _readerCategoryRepository.GetAllAsync();
        }

        // Добавление новой категории читателей
        public async Task<Guid> AddAsync(string name, string description)
        {
            var (category, error) = ReaderCategory.Create(Guid.NewGuid(), name, description);
            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }

            return await _readerCategoryRepository.AddAsync(category);
        }

        // Обновление категории читателей
        public async Task<Guid> UpdateAsync(Guid id, string name, string description)
        {
            var (category, error) = ReaderCategory.Create(id, name, description);
            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }

            return await _readerCategoryRepository.UpdateAsync(category);
        }

        // Удаление категории читателей по ID
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _readerCategoryRepository.DeleteAsync(id);
        }
    }
}
