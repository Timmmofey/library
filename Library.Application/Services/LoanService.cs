using Library.Core.Abstaction;
using Library.Core.Models;

namespace Library.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        // Получение займа по ID
        public async Task<Loan?> GetByIdAsync(Guid loanId)
        {
            return await _loanRepository.GetByIdAsync(loanId);
        }

        // Получение всех займов
        public async Task<List<Loan>> GetAllAsync()
        {
            return await _loanRepository.GetAllAsync();
        }

        // Добавление нового займа
        public async Task<Guid> AddAsync(Loan loan)
        {
            var (createdLoan, error) = Loan.Create(
                Guid.NewGuid(),
                loan.ItemCopyId,
                loan.ReaderId,
                loan.LibrarianId,
                DateTime.SpecifyKind(loan.IssueDate.Date, DateTimeKind.Utc),
                DateTime.SpecifyKind(loan.DueDate.Date, DateTimeKind.Utc),
                loan.ReturnDate.HasValue ? DateTime.SpecifyKind(loan.ReturnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null,
                loan.Lost
            );

            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }

            return await _loanRepository.AddAsync(createdLoan);
        }

        // Обновление займа
        public async Task<Guid> UpdateAsync(Loan loan)
        {
            return await _loanRepository.UpdateAsync(loan);
        }

        // Удаление займа по ID
        public async Task<Guid> DeleteAsync(Guid loanId)
        {
            return await _loanRepository.DeleteAsync(loanId);
        }

        // Поиск займов по параметрам
        public async Task<List<Loan>> SearchLoansAsync(Guid? itemCopyId, Guid? readerId, Guid? librarianId, DateTime? issueDate, DateTime? dueDate, DateTime? returnDate, bool? lost)
        {
            // Приведение дат к UTC
            issueDate = issueDate.HasValue ? DateTime.SpecifyKind(issueDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            dueDate = dueDate.HasValue ? DateTime.SpecifyKind(dueDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            returnDate = returnDate.HasValue ? DateTime.SpecifyKind(returnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;

            return await _loanRepository.SearchLoansAsync(itemCopyId, readerId, librarianId, issueDate, dueDate, returnDate, lost);
        }

        // Принятие возврата займа
        public async Task<Guid> AcceptReturnAsync(Guid loanId)
        {
            return await _loanRepository.AcceptReturnAsync(loanId);
        }

        // Заявление о потере займа
        public async Task<Guid> ReportLostAsync(Guid loanId)
        {
            return await _loanRepository.ReportLostAsync(loanId);
        }
    }
}

