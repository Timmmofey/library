using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IItemCopyService _itemCopyService;


        public LoanController(ILoanService loanService, IItemCopyService itemCopyService)
        {
            _loanService = loanService;
            _itemCopyService = itemCopyService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanResponse>> GetByIdAsync(Guid id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            return Ok(new LoanResponse
            {
                Id = loan.Id,
                ItemCopyId = loan.ItemCopyId,
                ReaderId = loan.ReaderId,
                LibrarianId = loan.LibrarianId,
                IssueDate = loan.IssueDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Lost = loan.Lost
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<LoanResponse>>> GetAllAsync()
        {
            var loans = await _loanService.GetAllAsync();
            var loanResponses = loans.ConvertAll(loan => new LoanResponse
            {
                Id = loan.Id,
                ItemCopyId = loan.ItemCopyId,
                ReaderId = loan.ReaderId,
                LibrarianId = loan.LibrarianId,
                IssueDate = loan.IssueDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Lost = loan.Lost
            });

            return Ok(loanResponses);
        }

        [Authorize(Roles = "Librarian, Admin, Director")]
        [HttpPost]
        public async Task<ActionResult<LoanResponse>> AddLoanAsync([FromBody] LoanRequest request)
        {
            var librarianId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(librarianId))
            {
                return Unauthorized();
            }

            var itemCopy = await _itemCopyService.GetByIdAsync(request.ItemCopyId);

            if (itemCopy.Loaned == true)
            {
                return BadRequest("This book is already loaned");
            }

            var issueDateUtc = DateTime.UtcNow.Date;

            var dueDateUtc = DateTime.SpecifyKind(request.DueDate.Date, DateTimeKind.Utc);

            if (itemCopy.Loanable == false)
            {
                dueDateUtc = DateTime.UtcNow.Date;
            }

            var loan = Loan.Create(
                Guid.NewGuid(),
                request.ItemCopyId,
                request.ReaderId,
                Guid.Parse(librarianId),
                issueDateUtc,
                dueDateUtc,
                null,
                false
            ).Loan;

            var loanId = await _loanService.AddAsync(loan);

            await _itemCopyService.ReportLoan(request.ItemCopyId);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LoanResponse>> UpdateAsync(Guid id, [FromBody] LoanUpdateRequest request)
        {
            var issueDateUtc = DateTime.SpecifyKind(request.IssueDate.Date, DateTimeKind.Utc);
            var dueDateUtc = DateTime.SpecifyKind(request.DueDate.Date, DateTimeKind.Utc);
            var returnDateUtc = request.ReturnDate.HasValue ? DateTime.SpecifyKind(request.ReturnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;

            var loan = Loan.Create(
                id,
                request.ItemCopyId,
                request.ReaderId,
                request.LibrarianId,
                issueDateUtc,
                dueDateUtc,
                returnDateUtc,
                request.Lost
            ).Loan;

            await _loanService.UpdateAsync(loan);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _loanService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<LoanResponse>>> SearchLoansAsync(
            [FromQuery] Guid? itemCopyId,
            [FromQuery] Guid? readerId,
            [FromQuery] Guid? librarianId,
            [FromQuery] DateTime? issueDate,
            [FromQuery] DateTime? dueDate,
            [FromQuery] DateTime? returnDate,
            [FromQuery] bool? lost)
        {
            issueDate = issueDate.HasValue ? DateTime.SpecifyKind(issueDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            dueDate = dueDate.HasValue ? DateTime.SpecifyKind(dueDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;
            returnDate = returnDate.HasValue ? DateTime.SpecifyKind(returnDate.Value.Date, DateTimeKind.Utc) : (DateTime?)null;

            var loans = await _loanService.SearchLoansAsync(itemCopyId, readerId, librarianId, issueDate, dueDate, returnDate, lost);
            var loanResponses = loans.ConvertAll(loan => new LoanResponse
            {
                Id = loan.Id,
                ItemCopyId = loan.ItemCopyId,
                ReaderId = loan.ReaderId,
                LibrarianId = loan.LibrarianId,
                IssueDate = loan.IssueDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Lost = loan.Lost
            });

            return Ok(loanResponses);
        }

        [HttpPost("{id}/return")]
        public async Task<ActionResult> AcceptReturnAsync(Guid id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            var bookId = loan.ItemCopyId;
            await _itemCopyService.CancelLostAsync(bookId);
            await _itemCopyService.CancelLoan(bookId);
            await _loanService.AcceptReturnAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/reportLost")]
        public async Task<ActionResult> ReportLostAsync(Guid id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            var bookId = loan.ItemCopyId; 
            await _itemCopyService.ReportLostAsync(bookId);
            await _loanService.ReportLostAsync(id);
            return NoContent();
        }
    }
}

