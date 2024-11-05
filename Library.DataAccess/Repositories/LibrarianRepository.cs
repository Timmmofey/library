using Library.Core.Abstaction;
using Library.Core.DTOs;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class LibrarianRepository : ILibrarianRepository
    {
        private readonly LibraryDbContext _context;

        public LibrarianRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Librarian> GetByIdAsync(Guid id)
        {
            var librarianEntity = await _context.Librarians
                .Include(l => l.Loans)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (librarianEntity == null) return null;

            var librarian = Librarian.Create(
                librarianEntity.Id,
                librarianEntity.Name,
                librarianEntity.Login,
                librarianEntity.PasswordHash,
                librarianEntity.Role, 
                librarianEntity.LibraryId,
                librarianEntity.ReadingRoomId,
                librarianEntity.Loans.Select(loan => Loan.Create(
                    loan.Id,
                    loan.ItemCopyId,
                    loan.ReaderId,
                    loan.LibrarianId,
                    loan.IssueDate,
                    loan.DueDate,
                    loan.ReturnDate).Loan).ToList()
                ).Librarian;

            return librarian;
        }

        public async Task<List<Librarian>> GetAllAsync()
        {
            var librariesEntities = await _context.Librarians
                .Include(l => l.Loans)
                .AsNoTracking()
                .ToListAsync();

            var librarians = librariesEntities
                .Select(librarian => Librarian.Create(
                    librarian.Id,
                    librarian.Name,
                    librarian.Login,
                    librarian.PasswordHash,
                    librarian.Role,
                    librarian.LibraryId,
                    librarian.ReadingRoomId,
                    librarian.Loans.Select(loan => Loan.Create(
                        loan.Id,
                        loan.ItemCopyId,
                        loan.ReaderId,
                        loan.LibrarianId,
                        loan.IssueDate,
                        loan.DueDate,
                        loan.ReturnDate).Loan).ToList()
                    ).Librarian)
                .ToList();

            return librarians;
        }

        public async Task<Guid> AddAsync(Librarian librarian)
        {
            var librarianEntity = new LibrarianEntity
            {
                Id = librarian.Id,
                Name = librarian.Name,
                Login = librarian.Login,
                PasswordHash = librarian.PasswordHash,
                Role = librarian.Role, 
                ReadingRoomId = librarian.ReadingRoomId,
            };

            await _context.Librarians.AddAsync(librarianEntity);
            await _context.SaveChangesAsync();

            return librarianEntity.Id;
        }

        public async Task<Guid> AddDirector(Librarian director)
        {
            if (await _context.Librarians.AnyAsync(l => l.Login == director.Login))
            {
                throw new InvalidOperationException("A user with this login already exists.");
            }

            var librarianEntity = new LibrarianEntity
            {
                Id = director.Id,
                Name = director.Name,
                Login = director.Login,
                PasswordHash = director.PasswordHash,
                Role = Role.Director,
                LibraryId = director.LibraryId
            };

            await _context.Librarians.AddAsync(librarianEntity);
            await _context.SaveChangesAsync();

            return librarianEntity.Id;
        }

        public async Task<Guid> AddLibrarian(Librarian librarian)
        {
            if (await _context.Librarians.AnyAsync(l => l.Login == librarian.Login))
            {
                throw new InvalidOperationException("A user with this login already exists.1");
            }

            var librarianEntity = new LibrarianEntity
            {
                Id = librarian.Id,
                Name = librarian.Name,
                Login = librarian.Login,
                PasswordHash = librarian.PasswordHash,
                Role = Role.Librarian,
                ReadingRoomId = librarian.ReadingRoomId,
            };

            await _context.Librarians.AddAsync(librarianEntity);
            await _context.SaveChangesAsync();

            return librarianEntity.Id;
        }

        public async Task<Guid> UpdateAsync(Librarian librarian)
        {
            await _context.Librarians
                .Where(l => l.Id == librarian.Id)
                .ExecuteUpdateAsync(i => i
                    .SetProperty(l => l.Name, librarian.Name)
                    .SetProperty(l => l.ReadingRoomId, librarian.ReadingRoomId)
                    .SetProperty(l => l.Login, librarian.Login)
                    .SetProperty(l => l.PasswordHash, librarian.PasswordHash)
                    .SetProperty(l => l.Role, librarian.Role));  

            return librarian.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.Librarians
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        // Поиск библиотекарей по имени и названию библиотеки
        //public async Task<List<LibrarianDto>> SearchLibrariansAsync(string? name, string? libraryName)
        //{
        //    var query = _context.Librarians
        //        .Include(l => l.Loans)
        //        .Include(l => l.ReadingRoom)
        //        .ThenInclude(rr => rr.Library)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        query = query.Where(l => l.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(libraryName))
        //    {
        //        query = query.Where(l => l.ReadingRoom.Library.Name.Trim().ToLower().Contains(libraryName.Trim().ToLower()));
        //    }

        //    var librariesEntities = await query.AsNoTracking().ToListAsync();

        //    var librarians = librariesEntities
        //        .Select(l => new LibrarianDto
        //        {
        //            Id = l.Id,
        //            Name = l.Name,
        //            ReadingRoomName = l.ReadingRoom.Name,
        //            LibraryName = l.ReadingRoom.Library.Name,
        //            Role = l.Role,  // Добавлено поле роли
        //            Loans = l.Loans.Select(loan => new LoanDto
        //            {
        //                Id = loan.Id,
        //                ItemCopyId = loan.ItemCopyId,
        //                ReaderId = loan.ReaderId,
        //                IssueDate = loan.IssueDate,
        //                DueDate = loan.DueDate,
        //                ReturnDate = loan.ReturnDate
        //            }).ToList()
        //        })
        //        .ToList();

        //    return librarians;
        //}

        public async Task<List<LibrarianDto1>> SearchLibrariansAsync(string? name, string? libraryName)
        {
            var query = _context.Librarians
                .Include(l => l.Loans)
                .Include(l => l.ReadingRoom)
                    .ThenInclude(rr => rr.Library)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(l => l.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(libraryName))
            {
                query = query.Where(l => l.ReadingRoom.Library.Name.Trim().ToLower().Contains(libraryName.Trim().ToLower()));
            }

            var librariesEntities = await query.AsNoTracking().ToListAsync();

            //var librarians = librariesEntities
            //    .Select(l => new LibrarianDto1(
            //        l.Id,
            //        l.Name,
            //        l.ReadingRoom?.Name,
            //        l.ReadingRoom?.Library?.Name,
            //        l.Role,
            //        l.Loans.Select(loan => new LoanDto
            //        {
            //            Id = loan.Id,
            //            ItemCopyId = loan.ItemCopyId,
            //            ReaderId = loan.ReaderId,
            //            IssueDate = loan.IssueDate,
            //            DueDate = loan.DueDate,
            //            ReturnDate = loan.ReturnDate
            //        }).ToList()
            //    ))
            //    .ToList();

            var librarians = await _context.Librarians
                .Select(l => new LibrarianDto1
                {
                    Id = l.Id,
                    Name = l.Name,
                    Login = l.Login,
                    LibraryId = l.LibraryId,
                    Role = l.Role
                })
                .ToListAsync();

            return librarians;
        }

        // Получение библиотекаря по логину
        public async Task<Librarian?> GetByLogin(string login)
        {
            var librarianEntity = await _context.Librarians
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Login == login);

            if (librarianEntity == null) return null;

            var librarian = Librarian.Create(
               librarianEntity.Id,
               librarianEntity.Name,
               librarianEntity.Login,
               librarianEntity.PasswordHash,
               librarianEntity.Role,
               librarianEntity.LibraryId,
               librarianEntity.ReadingRoomId
               ).Librarian;

            return librarian;
        }

        public async Task<Guid> EditInfo(Guid id, string? name, string? login, string? passwordHash,  Guid? libraryId, Guid? ReadingRoomId)
        {
            var worker = await _context.Librarians.FindAsync(id);

            if (worker == null) throw new KeyNotFoundException("Worker not found.");

            worker.Name = name ?? worker.Name;
            worker.Login = login ?? worker.Login;
            worker.PasswordHash = passwordHash ?? worker.PasswordHash;
            worker.LibraryId = libraryId ?? worker.LibraryId;
            worker.ReadingRoomId = ReadingRoomId;

            return id;
        }

    }
}
