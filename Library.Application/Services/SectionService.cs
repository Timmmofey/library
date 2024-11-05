using Library.Core.Abstaction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        // Получение секции по ID
        public async Task<Section> GetByIdAsync(Guid id)
        {
            return await _sectionRepository.GetByIdAsync(id);
        }

        // Получение всех секций
        public async Task<List<Section>> GetAllAsync()
        {
            return await _sectionRepository.GetAllAsync();
        }

        // Добавление новой секции
        public async Task<Guid> AddAsync(Section section)
        {
            var (newSection, error) = Section.Create(section.Id, section.ReadingRoomId, section.Name);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);  // Обработка ошибки создания секции
            }

            return await _sectionRepository.AddAsync(newSection);
        }

        // Обновление секции
        public async Task<Guid> UpdateAsync(Section section)
        {
            var (updatedSection, error) = Section.Create(section.Id, section.ReadingRoomId, section.Name);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);  // Обработка ошибки обновления секции
            }

            return await _sectionRepository.UpdateAsync(updatedSection);
        }

        // Удаление секции по ID
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _sectionRepository.DeleteAsync(id);
        }

        // Поиск секций по имени
        public async Task<List<Section>> SearchSectionsAsync(string? name)
        {
            return await _sectionRepository.SearchSectionsAsync(name);
        }
    }
}
