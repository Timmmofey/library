using Library.Core.Abstaction;
using Library.Core.DTOs;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ReadingRoomRepository : IReadingRoomRepository
    {
        private readonly LibraryDbContext _context;

        public ReadingRoomRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ReadingRoom> GetByIdAsync(Guid id)
        {
            var readingRoomEntity = await _context.ReadingRooms
                .Include(r => r.Librarians)
                .Include(r => r.Sections)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (readingRoomEntity == null) return null;

            var readingRoom = ReadingRoom.Create(
                readingRoomEntity.Id,
                readingRoomEntity.Name,
                readingRoomEntity.LibraryId,
                readingRoomEntity.Librarians.Select(l => Librarian.Create(l.Id, l.Name, l.Login, l.PasswordHash, l.Role, l.ReadingRoomId).Librarian).ToList(),
                readingRoomEntity.Sections.Select(s => Section.Create(s.Id, s.ReadingRoomId, s.Name).Section).ToList()
            ).ReadingRoom;

            return readingRoom;
        }

        public async Task<List<ReadingRoom>> GetAllAsync()
        {
            var readingRoomEntities = await _context.ReadingRooms
                .Include(r => r.Librarians)
                .Include(r => r.Sections)
                .AsNoTracking()
                .ToListAsync();

            var readingRooms = readingRoomEntities.Select(r => ReadingRoom.Create(
                r.Id,
                r.Name,
                r.LibraryId,
                r.Librarians.Select(l => Librarian.Create(l.Id, l.Name, l.Login, l.PasswordHash, l.Role, l.ReadingRoomId).Librarian).ToList(),
                r.Sections.Select(s => Section.Create(s.Id, s.ReadingRoomId, s.Name).Section).ToList()
            ).ReadingRoom).ToList();

            return readingRooms;
        }

        public async Task<Guid> AddAsync(ReadingRoom readingRoom)
        {
            var readingRoomEntity = new ReadingRoomEntity
            {
                Id = readingRoom.Id,
                Name = readingRoom.Name,
                LibraryId = readingRoom.LibraryId,
            };

            await _context.ReadingRooms.AddAsync(readingRoomEntity);
            await _context.SaveChangesAsync();

            return readingRoom.Id;
        }

        public async Task<Guid> UpdateAsync(ReadingRoom readingRoom)
        {
            await _context.ReadingRooms
                .Where(r => r.Id == readingRoom.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.Name, readingRoom.Name)
                .SetProperty(r => r.LibraryId, readingRoom.LibraryId));

            return readingRoom.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.ReadingRooms
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<List<ReadingRoom>> SearchReadingRoomsAsync(string? name)
        {
            var query = _context.ReadingRooms.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            }

            var readingRoomEntities = await query.AsNoTracking().ToListAsync();

            var readingRooms = readingRoomEntities.Select(r => ReadingRoom.Create(
                r.Id,
                r.Name,
                r.LibraryId,
                r.Librarians.Select(l => Librarian.Create(l.Id, l.Name, l.Login, l.PasswordHash, l.Role, l.ReadingRoomId).Librarian).ToList(),
                r.Sections.Select(s => Section.Create(s.Id, s.ReadingRoomId, s.Name).Section).ToList()
            ).ReadingRoom).ToList();

            return readingRooms;
        }
    }
}
