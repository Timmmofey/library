using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Loan?> GetByIdAsync(Guid loanId)
        {
            var loanEntity = await _context.Loans
                .AsNoTracking()
                .Include(l => l.ItemCopy)
                .Include(l => l.Reader)
                .Include(l => l.Librarian)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loanEntity == null)
            {
                return null;
            }

            var loan = Loan.Create(
                loanEntity.Id,
                loanEntity.ItemCopyId,
                loanEntity.ReaderId,
                loanEntity.LibrarianId,
                loanEntity.IssueDate,
                loanEntity.DueDate,
                loanEntity.ReturnDate,
                loanEntity.Lost
            ).Loan;

            return loan;
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            var loanEntities = await _context.Loans
                .AsNoTracking()
                .Include(l => l.ItemCopy)
                .Include(l => l.Reader)
                .Include(l => l.Librarian)
                .ToListAsync();

            var loans = loanEntities.Select(loanEntity =>
                Loan.Create(
                    loanEntity.Id,
                    loanEntity.ItemCopyId,
                    loanEntity.ReaderId,
                    loanEntity.LibrarianId,
                    loanEntity.IssueDate,
                    loanEntity.DueDate,
                    loanEntity.ReturnDate,
                    loanEntity.Lost
                ).Loan
            ).ToList();

            return loans;
        }

        public async Task<Guid> AddAsync(Loan loan)
        {
            var loanEntity = new LoanEntity
            {
                Id = loan.Id,
                ItemCopyId = loan.ItemCopyId,
                ReaderId = loan.ReaderId,
                LibrarianId = loan.LibrarianId,

                IssueDate = DateTime.SpecifyKind(loan.IssueDate.Date, DateTimeKind.Utc),
                DueDate = DateTime.SpecifyKind(loan.DueDate.Date, DateTimeKind.Utc),
                //ReturnDate = loan.ReturnDate.HasValue ? DateTime.SpecifyKind(loan.ReturnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null,
                //Lost = loan.Lost
            };

            await _context.Loans.AddAsync(loanEntity);
            await _context.SaveChangesAsync();

            return loan.Id;
        }

        public async Task<Guid> UpdateAsync(Loan loan)
        {
            var loanEntity = await _context.Loans.FindAsync(loan.Id);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.ItemCopyId = loan.ItemCopyId;
            loanEntity.ReaderId = loan.ReaderId;
            loanEntity.LibrarianId = loan.LibrarianId;
            loanEntity.IssueDate = loan.IssueDate;
            loanEntity.DueDate = loan.DueDate;
            loanEntity.ReturnDate = loan.ReturnDate;
            loanEntity.Lost = loan.Lost;

            await _context.SaveChangesAsync();

            return loan.Id;
        }

        public async Task<Guid> DeleteAsync(Guid loanId)
        {
            var loanEntity = await _context.Loans.FindAsync(loanId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            _context.Loans.Remove(loanEntity);
            await _context.SaveChangesAsync();

            return loanId;
        }

        public async Task<List<Loan>> SearchLoansAsync(Guid? itemCopyId, Guid? readerId, Guid? librarianId, DateTime? issueDate, DateTime? dueDate, DateTime? returnDate, bool? lost)
        {
            var query = _context.Loans
                .AsNoTracking()
                .AsQueryable();

            if (itemCopyId.HasValue)
            {
                query = query.Where(l => l.ItemCopyId == itemCopyId.Value);
            }

            if (readerId.HasValue)
            {
                query = query.Where(l => l.ReaderId == readerId.Value);
            }

            if (librarianId.HasValue)
            {
                query = query.Where(l => l.LibrarianId == librarianId.Value);
            }

            if (issueDate.HasValue)
            {
                query = query.Where(l => l.IssueDate.Date == issueDate.Value.Date);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(l => l.DueDate.Date == dueDate.Value.Date);
            }

            if (returnDate.HasValue)
            {
                query = query.Where(l => l.ReturnDate.HasValue && l.ReturnDate.Value.Date == returnDate.Value.Date);
            }

            if (lost.HasValue)
            {
                query = query.Where(l => l.Lost.HasValue && l.Lost.Value == lost.Value);
            }

            var loanEntities = await query.ToListAsync();

            var loans = loanEntities.Select(loanEntity =>
                Loan.Create(
                    loanEntity.Id,
                    loanEntity.ItemCopyId,
                    loanEntity.ReaderId,
                    loanEntity.LibrarianId,
                    loanEntity.IssueDate,
                    loanEntity.DueDate,
                    loanEntity.ReturnDate,
                    loanEntity.Lost
                ).Loan
            ).ToList();

            return loans;
        }

        // Метод для принятия возврата
        public async Task<Guid> AcceptReturnAsync(Guid loanId)
        {
            var loanEntity = await _context.Loans.FindAsync(loanId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.ReturnDate = DateTime.UtcNow; // Устанавливаем дату возврата
            loanEntity.Lost = false; // Устанавливаем статус как не потеряна

            await _context.SaveChangesAsync();

            return loanId;
        }

        // Метод для заявления о потере книги
        public async Task<Guid> ReportLostAsync(Guid loanId)
        {
            var loanEntity = await _context.Loans.FindAsync(loanId);
            if (loanEntity == null) throw new KeyNotFoundException("Loan not found.");

            loanEntity.Lost = true; // Устанавливаем статус как потеряна

            await _context.SaveChangesAsync();

            return loanId;
        }
    }
}
