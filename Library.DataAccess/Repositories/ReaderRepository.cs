using Library.Core.Abstaction;
using Library.Core.Models;
using Library.Core.DTOs;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDbContext _context;

        public ReaderRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<Reader> GetByIdAsync(Guid id)
        {
            var readerEntity = await _context.Readers
                .Include(r => r.Loans) 
                .ThenInclude(l => l.ItemCopy) 
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (readerEntity == null) return null;

            var loans = readerEntity.Loans?.Select(loanEntity => Loan.Create(
                loanEntity.Id,
                loanEntity.ItemCopyId,
                loanEntity.ReaderId,
                loanEntity.LibrarianId,
                loanEntity.IssueDate,
                loanEntity.DueDate,
                loanEntity.ReturnDate
            ).Loan).ToList();

            var reader = Reader.Create(
                readerEntity.Id,
                readerEntity.Email,
                readerEntity.PasswordHash,
                readerEntity.FullName,
                readerEntity.LibraryId,
                readerEntity.ReaderCategoryId,
                loans,
                readerEntity.SubscriptionEndDate,
                readerEntity.EducationalInstitution,
                readerEntity.Faculty,
                readerEntity.Course,
                readerEntity.GroupNumber,
                readerEntity.Organization,
                readerEntity.ResearchTopic
            ).Reader;

            return reader;
        }
        public async Task<List<Reader>> GetAllAsync()
        {
            var readerEntities = await _context.Readers
                .Include(r => r.Loans) 
                .ThenInclude(l => l.ItemCopy)
                .AsNoTracking()
                .ToListAsync();

            var readers = readerEntities.Select(r =>
            {
                var loans = r.Loans?.Select(loanEntity => Loan.Create(
                    loanEntity.Id,
                    loanEntity.ItemCopyId,
                    loanEntity.ReaderId,
                    loanEntity.LibrarianId,
                    loanEntity.IssueDate,
                    loanEntity.DueDate,
                    loanEntity.ReturnDate
                ).Loan).ToList();

                return Reader.Create(
                    r.Id,
                    r.Email,
                    r.PasswordHash,
                    r.FullName,
                    r.LibraryId,
                    r.ReaderCategoryId,
                    loans,
                    r.SubscriptionEndDate,
                    r.EducationalInstitution,
                    r.Faculty,
                    r.Course,
                    r.GroupNumber,
                    r.Organization,
                    r.ResearchTopic
                ).Reader;
            }).ToList();

            return readers;
        }

        public async Task<Guid> AddReader(Reader reader)
        {
            if (await _context.Readers.AnyAsync(l => l.Email == reader.Email))
            {
                throw new InvalidOperationException("A user with this login already exists.");
            }

            var readerEntity = new ReaderEntity
            {
                Id = reader.Id,
                Email = reader.Email,
                PasswordHash = reader.PasswordHash,
                FullName = reader.FullName,
                LibraryId = reader.LibraryId,
                Role = Role.Reader
            };

            await _context.Readers.AddAsync(readerEntity);
            await _context.SaveChangesAsync();

            return reader.Id;
        }

        public async Task<Guid> UpdateAsync(Reader reader)
        {
            var readerEntity = await _context.Readers.FindAsync(reader.Id);
            if (readerEntity == null) throw new KeyNotFoundException("Reader not found.");

            readerEntity.FullName = reader.FullName;
            readerEntity.LibraryId = reader.LibraryId;
            readerEntity.ReaderCategoryId = reader.ReaderCategoryId;
            readerEntity.SubscriptionEndDate = reader.SubscriptionEndDate;
            readerEntity.EducationalInstitution = reader.EducationalInstitution;
            readerEntity.Faculty = reader.Faculty;
            readerEntity.Course = reader.Course;
            readerEntity.GroupNumber = reader.GroupNumber;
            readerEntity.Organization = reader.Organization;
            readerEntity.ResearchTopic = reader.ResearchTopic;

            await _context.SaveChangesAsync();

            return reader.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var readerEntity = await _context.Readers.FindAsync(id);
            if (readerEntity == null) throw new KeyNotFoundException("Reader not found.");

            _context.Readers.Remove(readerEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<List<Reader>> SearchReadersAsync(
            string? fullName,
            Guid? libraryId,
            Guid? ReaderCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic
            )
        {
            var query = _context.Readers
                .Include(r => r.Loans)
                .ThenInclude(l => l.ItemCopy)
                .AsQueryable();

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.Where(r => r.FullName.Trim().ToLower().Contains(fullName.Trim().ToLower()));
            }

            if (libraryId.HasValue)
            {
                query = query.Where(r => r.LibraryId == libraryId.Value);
            }

            if (ReaderCategoryId.HasValue)
            {
                query = query.Where(r => r.ReaderCategoryId == ReaderCategoryId.Value);
            }

            if (!string.IsNullOrEmpty(educationalInstitution))
            {
                query = query.Where(r => r.EducationalInstitution.Trim().ToLower().Contains(educationalInstitution.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(faculty))
            {
                query = query.Where(r => r.Faculty.Trim().ToLower().Contains(faculty.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(course))
            {
                query = query.Where(r => r.Course.Trim().ToLower().Contains(course.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(groupNumber))
            {
                query = query.Where(r => r.GroupNumber.Trim().ToLower().Contains(groupNumber.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(organization))
            {
                query = query.Where(r => r.Organization.Trim().ToLower().Contains(organization.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(researchTopic))
            {
                query = query.Where(r => r.ResearchTopic.Trim().ToLower().Contains(researchTopic.Trim().ToLower()));
            }

            var readerEntities = await query.AsNoTracking().ToListAsync();

            var readers = readerEntities.Select(r =>
            {
                var loans = r.Loans?.Select(loanEntity => Loan.Create(
                    loanEntity.Id,
                    loanEntity.ItemCopyId,
                    loanEntity.ReaderId,
                    loanEntity.LibrarianId,
                    loanEntity.IssueDate,
                    loanEntity.DueDate,
                    loanEntity.ReturnDate
                ).Loan).ToList();

                return Reader.Create(
                    r.Id,
                    r.Email,
                    r.PasswordHash,
                    r.FullName,
                    r.LibraryId,
                    r.ReaderCategoryId,
                    loans,
                    r.SubscriptionEndDate,
                    r.EducationalInstitution,
                    r.Faculty,
                    r.Course,
                    r.GroupNumber,
                    r.Organization,
                    r.ResearchTopic
                ).Reader;
            }).ToList();

            return readers;
        }

        public async Task<Reader?> GetByEmail(string email)
        {
            var readerEntity = await _context.Readers
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Email == email);

            if (readerEntity == null) return null;

            var reader = Reader.Create(
                readerEntity.Id,
                readerEntity.Email,
                readerEntity.PasswordHash,
                readerEntity.FullName,
                readerEntity.LibraryId,
                readerEntity.ReaderCategoryId,
                null,
                readerEntity.SubscriptionEndDate,
                readerEntity.EducationalInstitution,
                readerEntity.Faculty,
                readerEntity.Course,
                readerEntity.GroupNumber,
                readerEntity.Organization,
                readerEntity.ResearchTopic
                ).Reader;

            return reader;
        }


        public async Task<Guid> ExtendSubscription(Guid id, int days)
        {
            var reader = await _context.Readers.FindAsync(id);
            if(reader == null) throw new KeyNotFoundException("Loan not found.");

            if (reader.SubscriptionEndDate.HasValue && reader.SubscriptionEndDate.Value > DateTime.UtcNow)
            {
                reader.SubscriptionEndDate = reader.SubscriptionEndDate.Value.AddDays(days);
            }
            else
            {
                reader.SubscriptionEndDate = DateTime.UtcNow.AddDays(days);
            }

            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<Guid> EditMainInfo(Guid id, string? email, string? passwordHash, string? fullName, Guid? libraryId)
        {
            var reader = await _context.Readers.FindAsync(id);

            if (reader == null) throw new KeyNotFoundException("User not found.");

            reader.Email = email ?? reader.Email;
            reader.PasswordHash = passwordHash ?? reader.PasswordHash;
            reader.FullName = fullName ?? reader.FullName;

            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> EditAdditionalInfo(Guid id, 
                       Guid? readerCategoryId, 
                       string? educationalInstitution,
                       string? faculty,
                       string? course,
                       string? groupNumber,
                       string? organization,
                       string? researchTopic)
        {
            var reader = await _context.Readers.FindAsync(id);

            if (reader == null) throw new KeyNotFoundException("Reader not found.");

            reader.ReaderCategoryId = readerCategoryId ?? reader.ReaderCategoryId;
            reader.EducationalInstitution = educationalInstitution ?? reader.EducationalInstitution;
            reader.Faculty = faculty ?? reader.Faculty;
            reader.Course = course ?? reader.Course;
            reader.GroupNumber = groupNumber ?? reader.GroupNumber;
            reader.Organization = organization ?? reader.Organization;

            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> LoginExistsAsync(string email)
        {
            return await _context.Readers.AnyAsync(r => r.Email == email);
        }

        public async Task<List<string>> GetExistingEducationalInstitutionsAsync()
        {
            return await _context.Readers
                .Where(r => !string.IsNullOrEmpty(r.EducationalInstitution))
                .Select(r => r.EducationalInstitution.Trim())
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }

        public async Task<List<string>> GetExistingFacultiesAsync()
        {
            return await _context.Readers
                .Where(r => !string.IsNullOrEmpty(r.Faculty))
                .Select(r => r.Faculty.Trim())
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();
        }

        //public async Task<List<Reader>> SearchReadersAsync1(
        //    string? fullName,
        //    Guid? libraryId,
        //    Guid? ReaderCategoryId,
        //    string? educationalInstitution,
        //    string? faculty,
        //    string? course,
        //    string? groupNumber,
        //    string? organization,
        //    string? researchTopic
        //    )
        //{
        //    var query = _context.Readers
        //        .Include(r => r.Loans)
        //        .ThenInclude(l => l.ItemCopy)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(fullName))
        //    {
        //        query = query.Where(r => r.FullName.Trim().ToLower().Contains(fullName.Trim().ToLower()));
        //    }

        //    if (libraryId.HasValue)
        //    {
        //        query = query.Where(r => r.LibraryId == libraryId.Value);
        //    }

        //    if (ReaderCategoryId.HasValue)
        //    {
        //        query = query.Where(r => r.ReaderCategoryId == ReaderCategoryId.Value);
        //    }

        //    if (!string.IsNullOrEmpty(educationalInstitution))
        //    {
        //        query = query.Where(r => r.EducationalInstitution.Trim().ToLower().Contains(educationalInstitution.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(faculty))
        //    {
        //        query = query.Where(r => r.Faculty.Trim().ToLower().Contains(faculty.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(course))
        //    {
        //        query = query.Where(r => r.Course.Trim().ToLower().Contains(course.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(groupNumber))
        //    {
        //        query = query.Where(r => r.GroupNumber.Trim().ToLower().Contains(groupNumber.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(organization))
        //    {
        //        query = query.Where(r => r.Organization.Trim().ToLower().Contains(organization.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(researchTopic))
        //    {
        //        query = query.Where(r => r.ResearchTopic.Trim().ToLower().Contains(researchTopic.Trim().ToLower()));
        //    }

        //    var readerEntities = await query.AsNoTracking().ToListAsync();

        //    var readers = readerEntities.Select(r =>
        //    {
        //        var loans = r.Loans?.Select(loanEntity => Loan.Create(
        //            loanEntity.Id,
        //            loanEntity.ItemCopyId,
        //            loanEntity.ReaderId,
        //            loanEntity.LibrarianId,
        //            loanEntity.IssueDate,
        //            loanEntity.DueDate,
        //            loanEntity.ReturnDate
        //        ).Loan).ToList();

        //        return Reader.Create(
        //            r.Id,
        //            r.Email,
        //            r.PasswordHash,
        //            r.FullName,
        //            r.LibraryId,
        //            r.ReaderCategoryId,
        //            loans,
        //            r.SubscriptionEndDate,
        //            r.EducationalInstitution,
        //            r.Faculty,
        //            r.Course,
        //            r.GroupNumber,
        //            r.Organization,
        //            r.ResearchTopic
        //        ).Reader;
        //    }).ToList();

        //    return readers;
        //}

                public async Task<List<ReaderDto1>> SearchReadersAsync1(
            string? fullName,
            Guid? libraryId,
            Guid? readerCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic
        )
        {
            var query = _context.Readers
                .Include(r => r.Library)
                .Include(r => r.Loans)
                .ThenInclude(l => l.ItemCopy)
                .AsQueryable();

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.Where(r => r.FullName.Trim().ToLower().Contains(fullName.Trim().ToLower()));
            }

            if (libraryId.HasValue)
            {
                query = query.Where(r => r.LibraryId == libraryId.Value);
            }

            if (readerCategoryId.HasValue)
            {
                query = query.Where(r => r.ReaderCategoryId == readerCategoryId.Value);
            }

            if (!string.IsNullOrEmpty(educationalInstitution))
            {
                query = query.Where(r => r.EducationalInstitution.Trim().ToLower().Contains(educationalInstitution.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(faculty))
            {
                query = query.Where(r => r.Faculty.Trim().ToLower().Contains(faculty.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(course))
            {
                query = query.Where(r => r.Course.Trim().ToLower().Contains(course.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(groupNumber))
            {
                query = query.Where(r => r.GroupNumber.Trim().ToLower().Contains(groupNumber.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(organization))
            {
                query = query.Where(r => r.Organization.Trim().ToLower().Contains(organization.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(researchTopic))
            {
                query = query.Where(r => r.ResearchTopic.Trim().ToLower().Contains(researchTopic.Trim().ToLower()));
            }

            var readerEntities = await query.AsNoTracking().ToListAsync();

            var readers = readerEntities.Select(r =>
            {
                var loans = r.Loans?.Select(loanEntity => Loan.Create(
                    loanEntity.Id,
                    loanEntity.ItemCopyId,
                    loanEntity.ReaderId,
                    loanEntity.LibrarianId,
                    loanEntity.IssueDate,
                    loanEntity.DueDate,
                    loanEntity.ReturnDate
                ).Loan).ToList();

                return new ReaderDto1
                {
                    Id = r.Id,
                    Email = r.Email,
                    FullName = r.FullName,
                    LibraryName = r.Library?.Name,
                    ReaderCategoryId = r.ReaderCategoryId,
                    SubscriptionEndDate = r.SubscriptionEndDate,
                    EducationalInstitution = r.EducationalInstitution,
                    Faculty = r.Faculty,
                    Course = r.Course,
                    GroupNumber = r.GroupNumber,
                    Organization = r.Organization,
                    ResearchTopic = r.ResearchTopic
                };
            }).ToList();

            return readers;
        }

        public async Task<List<Reader>> GetReadersWithBookAsync2(string publication)
        {
            var readerEntities = await _context.Loans
                .Include(l => l.Reader)
                .Include(l => l.ItemCopy)
                .ThenInclude(ic => ic.Item)
                .Where(l => l.ItemCopy.Item.Publications.Trim().ToLower().Contains(publication.Trim().ToLower()) && l.ReturnDate == null)
                .Select(l => l.Reader)
                .Distinct()
                .ToListAsync();

            var readers = readerEntities.Select(r => Reader.Create(
                r.Id,
                r.Email,
                r.PasswordHash,
                r.FullName,
                r.LibraryId,
                r.ReaderCategoryId,
                null,
                r.SubscriptionEndDate,
                r.EducationalInstitution,
                r.Faculty,
                r.Course,
                r.GroupNumber,
                r.Organization,
                r.ResearchTopic
            ).Reader).ToList();

            return readers;
        }

        //public async Task<List<Reader>> GetReadersWithItemAsync3(Guid itemId)
        //{
        //    var readerEntities = await _context.Loans
        //        .Include(l => l.Reader)
        //        .Include(l => l.ItemCopy)
        //        .ThenInclude(ic => ic.Item)
        //        .Where(l => l.ItemCopy.Item.Id == itemId && l.ReturnDate == null) 
        //        .Select(l => l.Reader)
        //        .Distinct()
        //        .ToListAsync();

        //    var readers = readerEntities.Select(r => Reader.Create(
        //        r.Id,
        //        r.Email,
        //        r.PasswordHash,
        //        r.FullName,
        //        r.LibraryId,
        //        r.ReaderCategoryId,
        //        null,
        //        r.SubscriptionEndDate,
        //        r.EducationalInstitution,
        //        r.Faculty,
        //        r.Course,
        //        r.GroupNumber,
        //        r.Organization,
        //        r.ResearchTopic
        //    ).Reader).ToList();

        //    return readers;
        //}

        public async Task<List<ReaderDto1>> GetReadersWithItemAsync3(Guid itemId)
        {
            var readerEntities = await _context.Loans
                .Include(l => l.Reader)
                .ThenInclude(r => r.Library)
                .Include(l => l.ItemCopy)
                .ThenInclude(ic => ic.Item)
                .Where(l => l.ItemCopy.Item.Id == itemId && l.ReturnDate == null)
                .Select(l => l.Reader)
                .Distinct()
                .ToListAsync();

            var readers = readerEntities.Select(r => new ReaderDto1
            {
                Id = r.Id,
                Email = r.Email,
                FullName = r.FullName,
                LibraryName = r.Library?.Name, 
                ReaderCategoryId = r.ReaderCategoryId,
                SubscriptionEndDate = r.SubscriptionEndDate,
                EducationalInstitution = r.EducationalInstitution,
                Faculty = r.Faculty,
                Course = r.Course,
                GroupNumber = r.GroupNumber,
                Organization = r.Organization,
                ResearchTopic = r.ResearchTopic
            }).ToList();

            return readers;
        }


        public async Task<List<ReaderWithItemDto>> GetReadersWithItemInDateRangeAsync4(Guid itemId, DateTime startDate, DateTime endDate)
        {
            var loanEntities = await _context.Loans
                .Include(l => l.Reader) 
                .Include(l => l.ItemCopy)
                    .ThenInclude(ic => ic.Item) 
                .Where(l => l.ItemCopy.Item.Id == itemId &&
                            l.IssueDate >= startDate.Date &&
                            l.IssueDate <= endDate.Date)
                .Select(l => new ReaderWithItemDto
                {
                    ReaderId = l.Reader.Id,
                    ReaderFullName = l.Reader.FullName,
                    ItemTitle = l.ItemCopy.Item.Title,
                    IssueDate = l.IssueDate
                })
                .ToListAsync();

            return loanEntities;
        }

        //public async Task<List<ItemCopyDto1>> GetItemsByReaderAndRegistrationStatusAsync56(
        //        Guid readerId,
        //        DateTime startDate,
        //        DateTime endDate,
        //        bool isRegistered)
        //{
        //    var query = _context.Loans
        //        .Include(l => l.ItemCopy)
        //            .ThenInclude(ic => ic.Shelf)
        //            .ThenInclude(s => s.Section)
        //            .ThenInclude(sec => sec.ReadingRoom)
        //        .Include(l => l.Reader)
        //        .Where(l => l.ReaderId == readerId &&
        //                    l.IssueDate >= startDate &&
        //                    l.IssueDate <= endDate);

        //    var readerEntity = await _context.Readers
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(r => r.Id == readerId);

        //    if (readerEntity == null)
        //    {
        //        throw new KeyNotFoundException("Reader not found.");
        //    }

        //    if (isRegistered)
        //    {
        //        query = query.Where(l => l.Reader.LibraryId == readerEntity.LibraryId);
        //    }
        //    else
        //    {
        //        query = query.Where(l => l.Reader.LibraryId != readerEntity.LibraryId);
        //    }

        //    var itemEntities = await query
        //        .Select(l => l.ItemCopy)
        //        .Distinct()
        //        .ToListAsync();

        //    var itemCopyDtos = itemEntities.Select(itemCopyEntity => new ItemCopyDto1
        //    {
        //        ItemCopyId = itemCopyEntity.Id,
        //        ItemId = itemCopyEntity.ItemId,
        //        Title = itemCopyEntity.Item.Title,
        //        Authors = itemCopyEntity.Item.Authors.Select(a => a.Name).ToList(),
        //        InventoryNumber = itemCopyEntity.InventoryNumber,
        //        Loanable = itemCopyEntity.Loanable,
        //        Loaned = itemCopyEntity.Loaned,
        //        Lost = itemCopyEntity.Lost,
        //        DateReceived = itemCopyEntity.DateReceived,
        //        DateWithdrawn = itemCopyEntity.DateWithdrawn,
        //        Publications = itemCopyEntity.Item.Publications
        //    }).ToList();

        //    return itemCopyDtos;
        //}

        //public async Task<List<ItemEntity>> GetPublicationsFromRegisteredLibraryAsync5(Guid readerId, DateTime startDate, DateTime endDate)
        //{
        //    return await _context.Loans
        //        .Where(l => l.ReaderId == readerId
        //                    && l.Reader.LibraryId == l.ItemCopy.Shelf.Section.ReadingRoom.LibraryId
        //                    && l.IssueDate >= startDate
        //                    && l.IssueDate <= endDate)
        //        .Select(l => l.ItemCopy.Item)
        //        .Distinct()
        //        .ToListAsync();
        //}

        //public async Task<List<ItemEntity>> GetPublicationsFromUnregisteredLibrariesAsync6(Guid readerId, DateTime startDate, DateTime endDate)
        //{
        //    return await _context.Loans
        //        .Where(l => l.ReaderId == readerId
        //                    && l.Reader.LibraryId != l.ItemCopy.Shelf.Section.ReadingRoom.LibraryId
        //                    && l.IssueDate >= startDate
        //                    && l.IssueDate <= endDate)
        //        .Select(l => l.ItemCopy.Item)
        //        .Distinct()
        //        .ToListAsync();
        //}

        public async Task<List<ItemCopyDto1>> GetPublicationsByReaderAndLibraryRegistrationAsync56(
                Guid readerId,
                DateTime startDate,
                DateTime endDate,
                bool fromRegisteredLibrary)
        {
            var query = _context.Loans
                .Where(l => l.ReaderId == readerId
                            && ((fromRegisteredLibrary && l.Reader.LibraryId == l.ItemCopy.Shelf.Section.ReadingRoom.LibraryId)
                                || (!fromRegisteredLibrary && l.Reader.LibraryId != l.ItemCopy.Shelf.Section.ReadingRoom.LibraryId))
                            && l.IssueDate >= startDate
                            && l.IssueDate <= endDate)
                .Select(l => new ItemCopyDto1
                {
                    ItemCopyId = l.ItemCopy.Id,
                    ItemId = l.ItemCopy.Item.Id,
                    Title = l.ItemCopy.Item.Title,
                    Authors = l.ItemCopy.Item.Authors.Select(a => a.Name).ToList(),
                    InventoryNumber = l.ItemCopy.InventoryNumber,
                    Loanable = l.ItemCopy.Loanable,
                    Loaned = l.ItemCopy.Loaned,
                    Lost = l.ItemCopy.Lost,
                    DateReceived = l.ItemCopy.DateReceived,
                    DateWithdrawn = l.ItemCopy.DateWithdrawn,
                    Publications = l.ItemCopy.Item.Publications
                })
                ;

            return await query.ToListAsync();
        }


        //public async Task<List<ItemCopy>> GetLoanedItemsByShelfAsync7(Guid shelfId)
        //{
        //    var loanedItems = await _context.ItemCopies
        //        .AsNoTracking()
        //        .Include(ic => ic.Item) 
        //        .Include(ic => ic.Shelf) 
        //        .Where(ic => ic.ShelfId == shelfId && ic.Loaned) 
        //        .ToListAsync();

        //    var itemCopies = new List<ItemCopy>();

        //    foreach (var itemCopyEntity in loanedItems)
        //    {
        //        var itemCopy = ItemCopy.Create(
        //            itemCopyEntity.Id,
        //            itemCopyEntity.ItemId,
        //            itemCopyEntity.ShelfId,
        //            itemCopyEntity.InventoryNumber,
        //            itemCopyEntity.Loanable,
        //            itemCopyEntity.Loaned,
        //            itemCopyEntity.Lost,
        //            itemCopyEntity.DateReceived?.Date,
        //            itemCopyEntity.DateWithdrawn?.Date
        //        ).ItemCopy;

        //        itemCopies.Add(itemCopy);
        //    }

        //    return itemCopies;
        //}

        public async Task<List<ItemCopyDto1>> GetLoanedItemsByShelfAsync7(Guid shelfId)
        {
            var itemCopyDtos = await _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item)
                .Where(ic => ic.ShelfId == shelfId && ic.Loaned)
                .Select(ic => new ItemCopyDto1
                {
                    ItemCopyId = ic.Id,
                    ItemId = ic.ItemId,
                    Title = ic.Item.Title,
                    Authors = ic.Item.Authors.Select(a => a.Name).ToList(),
                    InventoryNumber = ic.InventoryNumber,
                    Loanable = ic.Loanable,
                    Loaned = ic.Loaned,
                    Lost = ic.Lost,
                    DateReceived = ic.DateReceived,
                    DateWithdrawn = ic.DateWithdrawn,
                    Publications = ic.Item.Publications 
                })
                .ToListAsync();

            return itemCopyDtos;
        }



        public async Task<List<Reader>> GetReadersServicedByLibrarianAsync8(Guid librarianId, DateTime startDate, DateTime endDate)
        {
            var loanEntities = await _context.Loans 
                .AsNoTracking()
                .Include(l => l.Reader) 
                .Where(l => l.LibrarianId == librarianId && l.IssueDate >= startDate && l.DueDate <= endDate) 
                .ToListAsync();

            var readers = new List<Reader>();

            foreach (var loanEntity in loanEntities)
            {
                var reader = loanEntity.Reader;

                var (readerModel, error) = Reader.Create(
                    reader.Id,
                    reader.Email,
                    reader.PasswordHash,
                    reader.FullName,
                    reader.LibraryId,
                    reader.ReaderCategoryId,
                    null,
                    reader.SubscriptionEndDate,
                    reader.EducationalInstitution,
                    reader.Faculty,
                    reader.Course,
                    reader.GroupNumber,
                    reader.Organization,
                    reader.ResearchTopic
                );

                if (!string.IsNullOrEmpty(error))
                {
                    continue;
                }

                readers.Add(readerModel);
            }

            return readers.GroupBy(r => r.Id)
                  .Select(g => g.First())
                  .ToList();
        }

        public async Task<List<LibrarianWorkReport>> GetLibrarianWorkReportAsync9(DateTime startDate, DateTime endDate)
        {
            var librarianReports = await _context.Loans
                .AsNoTracking()
                .Include(l => l.Librarian) 
                .Where(l => l.IssueDate >= startDate && l.DueDate <= endDate) 
                .GroupBy(l => l.Librarian)
                .Select(g => new LibrarianWorkReport
                {
                    LibrarianId = g.Key.Id,
                    LibrarianName = g.Key.Name,
                    NumberOfServedReaders = g.Count() 
                })
                .ToListAsync();

            return librarianReports;
        }

        //public async Task<List<Reader>> GetReadersWithOverdueLoansAsync10()
        //{
        //    var overdueLoans = await _context.Loans
        //        .AsNoTracking()
        //        .Include(l => l.Reader) 
        //        .ThenInclude(r => r.Library) 
        //        .Where(l => l.DueDate < DateTime.UtcNow && l.ReturnDate == null) 
        //        .Select(l => l.Reader)
        //        .Distinct()
        //        .ToListAsync();

        //    var readers = overdueLoans.Select(readerEntity => Reader.Create(
        //        readerEntity.Id,
        //        readerEntity.Email,
        //        readerEntity.PasswordHash,
        //        readerEntity.FullName,
        //        readerEntity.LibraryId,
        //        readerEntity.ReaderCategoryId,
        //        null,
        //        readerEntity.SubscriptionEndDate,
        //        readerEntity.EducationalInstitution,
        //        readerEntity.Faculty,
        //        readerEntity.Course,
        //        readerEntity.GroupNumber,
        //        readerEntity.Organization,
        //        readerEntity.ResearchTopic
        //    ).Reader).ToList();

        //    return readers;
        //}

        public async Task<List<ReaderDto1>> GetReadersWithOverdueLoansAsync10()
        {
            var overdueLoans = await _context.Loans
                .AsNoTracking()
                .Include(l => l.Reader)
                .ThenInclude(r => r.Library)
                .Where(l => l.DueDate < DateTime.UtcNow && l.ReturnDate == null)
                .Select(l => new ReaderDto1
                {
                    Id = l.Reader.Id,
                    Email = l.Reader.Email,
                    FullName = l.Reader.FullName,
                    LibraryName = l.Reader.Library != null ? l.Reader.Library.Name : null,
                    ReaderCategoryId = l.Reader.ReaderCategoryId,
                    SubscriptionEndDate = l.Reader.SubscriptionEndDate,
                    EducationalInstitution = l.Reader.EducationalInstitution,
                    Faculty = l.Reader.Faculty,
                    Course = l.Reader.Course,
                    GroupNumber = l.Reader.GroupNumber,
                    Organization = l.Reader.Organization,
                    ResearchTopic = l.Reader.ResearchTopic
                })
                .Distinct()
                .ToListAsync();

            return overdueLoans;
        }

        public async Task<List<ItemCopyDto1>> GetItemCopiesReceivedOrWithdrawnWithDetailsAsync11(
            DateTime startDate,
            DateTime endDate,
            bool includeReceived = true,
            bool includeWithdrawn = false)
        {
            var query = _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item)
                .ThenInclude(i => i.Authors)
                .Where(ic =>
                    (includeReceived && ic.DateReceived.HasValue && ic.DateReceived.Value >= startDate && ic.DateReceived.Value <= endDate && ic.DateWithdrawn.Value == null) ||
                    (includeWithdrawn && ic.DateWithdrawn.HasValue && ic.DateWithdrawn.Value >= startDate && ic.DateWithdrawn.Value <= endDate)
                );

            var itemCopyEntities = await query.ToListAsync();

            var itemCopiesWithDetails = itemCopyEntities.Select(itemCopyEntity => new ItemCopyDto1
            {
                ItemCopyId = itemCopyEntity.Id,
                ItemId = itemCopyEntity.ItemId,
                Title = itemCopyEntity.Item.Title,
                Authors = itemCopyEntity.Item.Authors.Select(a => a.Name).ToList(),
                InventoryNumber = itemCopyEntity.InventoryNumber,
                Loanable = itemCopyEntity.Loanable,
                Loaned = itemCopyEntity.Loaned,
                Lost = itemCopyEntity.Lost,
                DateReceived = itemCopyEntity.DateReceived?.Date,
                DateWithdrawn = itemCopyEntity.DateWithdrawn?.Date,
                Publications = itemCopyEntity.Item.Publications
            }).ToList();

            return itemCopiesWithDetails;
        }

        public async Task<List<LibrarianDto1>> GetLibrariansByReadingRoomIdAsync12(Guid readingRoomId)
        {
            var librarians = await _context.Librarians
                .AsNoTracking()
                .Where(l => l.ReadingRoomId == readingRoomId)
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

        public async Task<List<ReaderDto>> GetReadersNotVisitedAsync13(DateTime sinceDate, DateTime toDate)
        {
            var readers = await _context.Readers
                .AsNoTracking()
                .Where(r => !r.Loans.Any(l => l.IssueDate >= sinceDate && l.IssueDate <= toDate))
                .Select(r => new ReaderDto
                {
                    Id = r.Id,
                    FullName = r.FullName,
                    Email = r.Email,
                    LibraryId = r.LibraryId,
                    SubscriptionEndDate = r.SubscriptionEndDate
                })
                .ToListAsync();

            return readers;
        }

        public async Task<List<ItemCopyDto1>> GetItemCopiesByPublicationAsync14(string publication)
        {
            var itemCopies = await _context.ItemCopies
                .AsNoTracking()
                .Include(ic => ic.Item) 
                .Include(ic => ic.Item.Authors) 
                .Where(ic => ic.Item.Publications.Trim().ToLower().Contains(publication.Trim().ToLower())) 
                .Select(ic => new ItemCopyDto1
                {
                    ItemCopyId = ic.Id,
                    ItemId = ic.ItemId,
                    Title = ic.Item.Title,
                    Authors = ic.Item.Authors.Select(a => a.Name).ToList(), 
                    InventoryNumber = ic.InventoryNumber,
                    Loanable = ic.Loanable,
                    Loaned = ic.Loaned,
                    Lost = ic.Lost,
                    DateReceived = ic.DateReceived,
                    DateWithdrawn = ic.DateWithdrawn,
                    Publications = ic.Item.Publications 
                })
                .ToListAsync();

            return itemCopies;
        }

        //public async Task<List<Item>> SearchItemsAsync15(List<Guid>? authorIds)
        //{
        //    var query = _context.Items
        //        .Include(i => i.Authors)
        //        .AsQueryable();

        //    if (authorIds != null && authorIds.Count > 0)
        //    {
        //        query = query.Where(i => i.Authors.Any(a => authorIds.Contains(a.Id)));
        //    }

        //    var itemEntities = await query.AsNoTracking().ToListAsync();

        //    var items = itemEntities.Select(itemEntity =>
        //    {
        //        var authors = itemEntity.Authors.Select(a => Author.Create(a.Id, a.Name, a.Description).Author).ToList();
        //        return Item.Create(itemEntity.Id, itemEntity.Title, itemEntity.CategoryId, itemEntity.Publications, authors).Item;
        //    }).ToList();

        //    return items;
        //}

        public async Task<List<ItemCopyDto1>> SearchItemsAsync15(List<Guid>? authorIds)
        {
            var query = _context.Items
                .Include(i => i.Authors)
                .Include(i => i.ItemCopies)
                .AsQueryable();

            if (authorIds != null && authorIds.Count > 0)
            {
                query = query.Where(i => i.Authors.Any(a => authorIds.Contains(a.Id)));
            }

            var itemEntities = await query.AsNoTracking().ToListAsync();

            var items = itemEntities.Select(itemEntity =>
            {
                return itemEntity.ItemCopies.Select(itemCopy => new ItemCopyDto1
                {
                    ItemCopyId = itemCopy.Id,
                    ItemId = itemEntity.Id,
                    Title = itemEntity.Title,
                    Authors = itemEntity.Authors.Select(a => a.Name).ToList(),
                    InventoryNumber = itemCopy.InventoryNumber,
                    Loanable = itemCopy.Loanable,
                    Loaned = itemCopy.Loaned,
                    Lost = itemCopy.Lost,
                    DateReceived = itemCopy.DateReceived,
                    DateWithdrawn = itemCopy.DateWithdrawn,
                    Publications = itemEntity.Publications
                }).ToList();
            }).SelectMany(dto => dto).ToList();

            return items;
        }


        public async Task<List<PopularPublicationDto>> GetMostPopularPublicationsAsync16()
        {
            var loansWithPublications = await _context.Loans
                .AsNoTracking()
                .Include(loan => loan.ItemCopy)
                .ThenInclude(copy => copy.Item)
                .ToListAsync();

            var publicationGroups = loansWithPublications
                .Select(loan => loan.ItemCopy.Item.Publications.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .SelectMany(publications => publications.Select(pub => pub.Trim()))
                .GroupBy(publication => publication)
                .Select(group => new
                {
                    Publication = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return publicationGroups
                .Select(x => new PopularPublicationDto
                {
                    Publication = x.Publication,
                    Count = x.Count
                })
                .ToList();
        }

        public async Task<List<ShelfDto>> GetShelvesByLibraryIdAsync(Guid libraryId)
        {
            var shelves = await _context.Shelfs
                .Include(s => s.Section)
                    .ThenInclude(sec => sec.ReadingRoom)
                .Include(s => s.ItemCopies) 
                .Where(s => s.Section.ReadingRoom.LibraryId == libraryId)
                .Select(s => new ShelfDto
                {
                    Id = s.Id,
                    SectionId = s.SectionId,
                    Number = s.Number
                })
                .ToListAsync();

            return shelves;
        }

        public async Task<List<ReadingRoomDto>> GetReadingRoomsByLibraryIdAsync(Guid libraryId)
        {
            var readingRooms = await _context.ReadingRooms
                .AsNoTracking()
                .Where(rr => rr.LibraryId == libraryId)
                .Select(rr => new ReadingRoomDto
                {
                    Id = rr.Id,
                    Name = rr.Name
                })
                .ToListAsync();

            return readingRooms;
        }


    }
}
