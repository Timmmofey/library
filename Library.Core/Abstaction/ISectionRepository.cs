using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface ISectionRepository
    {
        Task<Section> GetByIdAsync(Guid id);            
        Task<List<Section>> GetAllAsync(); 
        Task<Guid> AddAsync(Section section); 
        Task<Guid> UpdateAsync(Section section);  
        Task<Guid> DeleteAsync(Guid id);   
        Task<List<Section>> SearchSectionsAsync(string? name);
    }
}
