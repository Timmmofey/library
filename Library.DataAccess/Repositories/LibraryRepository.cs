using Library.Core.Abstraction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryDbContext _context;

        public LibraryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<LibraryModel> GetByIdAsync(Guid id)
        {
            var libraryEntity = await _context.Libraries
                .Include(l => l.ReadingRooms)
                .Include(l => l.Readers)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libraryEntity == null) return null;

            var libraryModel = LibraryModel.Create(
                libraryEntity.Id,
                libraryEntity.Name,
                libraryEntity.Address,
                libraryEntity.Description,
                libraryEntity.ReadingRooms.Select(readingRoom => ReadingRoom.Create(readingRoom.Id, readingRoom.Name, readingRoom.LibraryId).ReadingRoom).ToList(),
                libraryEntity.Readers.Select(reader => Reader.Create(reader.Id, reader.Email, reader.PasswordHash, reader.FullName, reader.LibraryId).Reader).ToList()
            ).Library;

            return libraryModel;
        }

        public async Task<List<LibraryModel>> GetAllAsync()
        {
            var librariesEntities = await _context.Libraries
                .Include(l => l.ReadingRooms)
                .Include(l => l.Readers)
                .AsNoTracking()
                .ToListAsync();

            var libraries = librariesEntities
                .Select(library => LibraryModel.Create(
                    library.Id,
                    library.Name,
                    library.Address,
                    library.Description,
                    library.ReadingRooms.Select(readingRoom => ReadingRoom.Create(readingRoom.Id, readingRoom.Name, readingRoom.LibraryId).ReadingRoom).ToList(),
                    library.Readers.Select(reader => Reader.Create(reader.Id, reader.Email, reader.PasswordHash, reader.FullName, reader.LibraryId).Reader).ToList()
                ).Library)
                .ToList();

            return libraries;
        }

        public async Task<Guid> AddAsync(LibraryModel libraryModel)
        {
            var libraryEntity = new LibraryEntity
            {
                Id = libraryModel.Id,
                Name = libraryModel.Name,
                Address = libraryModel.Address,
                Description = libraryModel.Description
                
            };

            await _context.Libraries.AddAsync(libraryEntity);
            await _context.SaveChangesAsync();

            return libraryModel.Id;
        }

        public async Task<Guid> UpdateAsync(LibraryModel libraryModel)
        {
            await _context.Libraries
                .Where(l => l.Id == libraryModel.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(l => l.Name, libraryModel.Name)
                .SetProperty(l => l.Address, libraryModel.Address)
                .SetProperty(l => l.Description, libraryModel.Description));

            return libraryModel.Id;
        }
        
        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.Libraries
                .Where (l => l.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
                
        public async Task<List<LibraryModel>> SearchLibrariesAsync(string? name, string? address, string? description)
        {
            var query = _context.Libraries
                .Include(l => l.ReadingRooms)
                .Include(l => l.Readers)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(l => l.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(l => l.Address.Trim().ToLower().Contains(address.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(l => l.Description.Trim().ToLower().Contains(description.Trim().ToLower()));
            }

            var librariesEntities = await query.AsNoTracking().ToListAsync();

            var libraries = librariesEntities
                .Select(l =>
                {
                    var readingRooms = l.ReadingRooms
                        .Select(r => ReadingRoom.Create(r.Id, r.Name, r.LibraryId).ReadingRoom)
                        .ToList();

                    var readers = l.Readers
                        .Select(r => Reader.Create(r.Id, r.Email, r.PasswordHash, r.FullName, r.LibraryId).Reader)
                        .ToList();

                    var (library, error) = LibraryModel.Create(
                        l.Id,
                        l.Name,
                        l.Address,
                        l.Description,
                        readingRooms,
                        readers
                    );

                    return library;
                })
                .ToList();

            return libraries;
        }

        public async Task<List<(Guid Id, string Name)>> GetLibraryIdsAndNamesAsync()
        {
            return await _context.Libraries
                .Select(library => new { library.Id, library.Name })
                .AsNoTracking()
                .ToListAsync()
                .ContinueWith(task => task.Result
                    .Select(l => (l.Id, l.Name))
                    .ToList());
        }
    }
}
