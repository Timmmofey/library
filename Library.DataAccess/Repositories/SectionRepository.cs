using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly LibraryDbContext _context;

        public SectionRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Section> GetByIdAsync(Guid id)
        {
            var sectionEntity = await _context.Sections
                .Include(s => s.Shelves) 
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sectionEntity == null) return null;

            var section = Section.Create(
                sectionEntity.Id,
                sectionEntity.ReadingRoomId,
                sectionEntity.Name,
                sectionEntity.Shelves.Select(shelf => Shelf.Create(
                    shelf.Id,
                    shelf.SectionId,
                    shelf.Number
                ).Shelf).ToList() 
            ).Section;

            return section;
        }

        public async Task<List<Section>> GetAllAsync()
        {
            var sectionEntities = await _context.Sections
                .Include(s => s.Shelves) 
                .AsNoTracking()
                .ToListAsync();

            var sections = sectionEntities.Select(s => Section.Create(
                s.Id,
                s.ReadingRoomId,
                s.Name,
                s.Shelves.Select(shelf => Shelf.Create(
                    shelf.Id,
                    shelf.SectionId,
                    shelf.Number
                ).Shelf).ToList() 
            ).Section).ToList();

            return sections;
        }

        public async Task<Guid> AddAsync(Section section)
        {
            var sectionEntity = new SectionEntity
            {
                Id = section.Id,
                ReadingRoomId = section.ReadingRoomId,
                Name = section.Name
            };

            await _context.Sections.AddAsync(sectionEntity);

            await _context.SaveChangesAsync();

            return section.Id;
        }

        public async Task<Guid> UpdateAsync(Section section)
        {
            await _context.Sections
                .Where(s => s.Id == section.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(sec => sec.Name, section.Name)
                .SetProperty(sec => sec.ReadingRoomId, section.ReadingRoomId));

            return section.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.Sections
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<List<Section>> SearchSectionsAsync(string? name)
        {
            var query = _context.Sections
                .Include(s => s.Shelves)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            }

            var sectionEntities = await query.AsNoTracking().ToListAsync();

            var sections = sectionEntities.Select(s => Section.Create(
                s.Id,
                s.ReadingRoomId,
                s.Name,
                s.Shelves?.Select(shelf => Shelf.Create(
                    shelf.Id,
                    shelf.SectionId,
                    shelf.Number
                ).Shelf).ToList()
            ).Section).ToList();

            return sections;
        }
    }
}
