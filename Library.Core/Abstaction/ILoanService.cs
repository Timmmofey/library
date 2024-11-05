using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface ILoanService
    {
        Task<Loan?> GetByIdAsync(Guid loanId);
        Task<List<Loan>> GetAllAsync();
        Task<Guid> AddAsync(Loan loan);
        Task<Guid> UpdateAsync(Loan loan);
        Task<Guid> DeleteAsync(Guid loanId);
        Task<List<Loan>> SearchLoansAsync(Guid? itemCopyId, Guid? readerId, Guid? librarianId, DateTime? issueDate, DateTime? dueDate, DateTime? returnDate, bool? lost);

        Task<Guid> AcceptReturnAsync(Guid loanId);
        Task<Guid> ReportLostAsync(Guid loanId);
    }
}
