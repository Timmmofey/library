using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.DTOs;
using Library.Core.Models;
using Library.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;

        public ReaderController(IReaderService readerService)
        {
            _readerService = readerService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReaderResponse>> GetById(Guid id)
        {
            var reader = await _readerService.GetByIdAsync(id);
            if (reader == null)
            {
                return NotFound();
            }

            var response = new ReaderResponse
            {
                Id = reader.Id,
                FullName = reader.FullName,
                LibraryId = reader.LibraryId,
                ReaderCategoryId = reader.ReaderCategoryId,
                SubscriptionEndDate = reader.SubscriptionEndDate,
                EducationalInstitution = reader.EducationalInstitution,
                Faculty = reader.Faculty,
                Course = reader.Course,
                GroupNumber = reader.GroupNumber,
                Organization = reader.Organization,
                ResearchTopic = reader.ResearchTopic,
                Loans = reader.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ReaderResponse>>> GetAll()
        {
            var readers = await _readerService.GetAllAsync();

            var response = readers.Select(r => new ReaderResponse
            {
                Id = r.Id,
                FullName = r.FullName,
                ReaderCategoryId = r.ReaderCategoryId,
                LibraryId = r.LibraryId,
                SubscriptionEndDate = r.SubscriptionEndDate,
                EducationalInstitution = r.EducationalInstitution,
                Faculty = r.Faculty,
                Course = r.Course,
                GroupNumber = r.GroupNumber,
                Organization = r.Organization,
                ResearchTopic = r.ResearchTopic,
                Loans = r.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpPost("addReader")]
        public async Task<ActionResult<Guid>> RegisterReader(ReaderRegisterRequest request)
        {
            var (reader, error) = Reader.Create(
                Guid.NewGuid(),
                request.Email,
                request.PasswordHash,
                request.FullName,
                request.LibraryId
            );

            try
            {
                var id = await _readerService.Register(reader);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ReaderRequest request)
        {
            var (reader, error) = Reader.Create(
                id,
                request.Email,
                request.PasswordHash,
                request.FullName,
                request.LibraryId,
                request.ReaderCategoryId,
                subscriptionEndDate: request.SubscriptionEndDate,
                educationalInstitution: request.EducationalInstitution,
                faculty: request.Faculty,
                course: request.Course,
                groupNumber: request.GroupNumber,
                organization: request.Organization,
                researchTopic: request.ResearchTopic
            );

            if (reader == null)
            {
                return BadRequest(error);
            }

            await _readerService.UpdateAsync(reader);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _readerService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ReaderResponse>>> Search([FromQuery] string? fullName,
            Guid? libraryId,
            Guid? ReaderCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic)
        {
            var readers = await _readerService.SearchReadersAsync(fullName,
            libraryId,
            ReaderCategoryId,
            educationalInstitution,
            faculty,
            course,
            groupNumber,
            organization,
            researchTopic);

            var response = readers.Select(r => new ReaderResponse
            {
                Id = r.Id,
                FullName = r.FullName,
                ReaderCategoryId = r.ReaderCategoryId,
                LibraryId = r.LibraryId,
                SubscriptionEndDate = r.SubscriptionEndDate,
                EducationalInstitution = r.EducationalInstitution,
                Faculty = r.Faculty,
                Course = r.Course,
                GroupNumber = r.GroupNumber,
                Organization = r.Organization,
                ResearchTopic = r.ResearchTopic,
                Loans = r.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("loginReader")]
        public async Task<IActionResult> LoginReader(LoginReaderRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Email and password are required." });
            }

            try
            {
                var token = await _readerService.Login(request.Email, request.Password);

                if (token == null)
                {
                    return Unauthorized(new { Message = "Invalid credentials." });
                }

                Response.Cookies.Append("some-cookie", token);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Логируйте здесь, чтобы увидеть больше информации
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Reader")]
        [HttpPost("extendSubscription")]
        public async Task<ActionResult<Guid>> ExtendSubscription(int days)
        {
            // Извлечение идентификатора пользователя из Claims
            var readerId = User.Claims.FirstOrDefault(r => r.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(readerId) || !Guid.TryParse(readerId, out var readerGuid))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _readerService.ExtendSubscription(readerGuid, days);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Reader not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while extending the subscription.", Details = ex.Message });
            }
        }

        [Authorize(Roles = "Reader")]
        [HttpPost("editMainInfo")]
        public async Task<ActionResult<Guid>> EditMainInfo(string? email, string? passwordHash, string? fullName, Guid? libraryId)
        {
            var readerId = User.Claims.FirstOrDefault(r => r.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(readerId) || !Guid.TryParse(readerId, out var readerGuid))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _readerService.EditMainInfo(readerGuid, email, passwordHash, fullName, libraryId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Reader not found." });
            }

        }

        [Authorize(Roles = "Reader")]
        [HttpPost("getReadersInfo")]
        public async Task<ActionResult<ReaderResponse>> GetReadersInfo()
        {
            var readerId = User.Claims.FirstOrDefault(r => r.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(readerId) || !Guid.TryParse(readerId, out var readerGuid))
            {
                return Unauthorized();
            }

            var reader = await _readerService.GetByIdAsync(readerGuid);
            if (reader == null)
            {
                return NotFound();
            }

            var response = new ReaderResponse
            {
                Id = reader.Id,
                FullName = reader.FullName,
                LibraryId = reader.LibraryId,
                ReaderCategoryId = reader.ReaderCategoryId,
                SubscriptionEndDate = reader.SubscriptionEndDate,
                EducationalInstitution = reader.EducationalInstitution,
                Faculty = reader.Faculty,
                Course = reader.Course,
                GroupNumber = reader.GroupNumber,
                Organization = reader.Organization,
                ResearchTopic = reader.ResearchTopic,
                Loans = reader.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("search1")]
        public async Task<IActionResult> SearchReadersAsync(
            [FromQuery] string? fullName,
            [FromQuery] Guid? libraryId,
            [FromQuery] Guid? readerCategoryId,
            [FromQuery] string? educationalInstitution,
            [FromQuery] string? faculty,
            [FromQuery] string? course,
            [FromQuery] string? groupNumber,
            [FromQuery] string? organization,
            [FromQuery] string? researchTopic)
        {
            var readers = await _readerService.SearchReadersAsync1(
                fullName,
                libraryId,
                readerCategoryId,
                educationalInstitution,
                faculty,
                course,
                groupNumber,
                organization,
                researchTopic);

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers found matching the specified criteria.");
            }

            return Ok(readers);
        }

        [HttpGet("withBook2")]
        public async Task<IActionResult> GetReadersWithBookAsync([FromQuery] string publication)
        {
            if (string.IsNullOrWhiteSpace(publication))
            {
                return BadRequest("Publication name is required.");
            }

            var readers = await _readerService.GetReadersWithBookAsync2(publication);

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers found with the specified publication.");
            }

            return Ok(readers);
        }

        [HttpGet("withItem3/{itemId}")]
        public async Task<IActionResult> GetReadersWithItemAsync(Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                return BadRequest("Item ID is required.");
            }

            var readers = await _readerService.GetReadersWithItemAsync3(itemId);

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers found with the specified item.");
            }

            return Ok(readers);
        }

        [HttpGet("readersWithItemInDateRange4")]
        public async Task<IActionResult> GetReadersWithItemInDateRangeAsync(Guid itemId, DateTime startDate, DateTime endDate)
        {
            if (itemId == Guid.Empty)
            {
                return BadRequest("Item ID is required.");
            }

            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            var readersWithItem = await _readerService.GetReadersWithItemInDateRangeAsync4(itemId, startDate, endDate);

            if (readersWithItem == null || readersWithItem.Count == 0)
            {
                return NotFound("No readers found with the specified item in the given date range.");
            }

            return Ok(readersWithItem);
        }

        [HttpGet("itemsByReaderAndRegistrationStatus56")]
        public async Task<IActionResult> GetItemsByReaderAndRegistrationStatusAsync(Guid readerId, DateTime startDate, DateTime endDate, bool isRegistered)
        {
            if (readerId == Guid.Empty)
            {
                return BadRequest("Reader ID is required.");
            }

            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            var items = await _readerService.GetItemsByReaderAndRegistrationStatusAsync56(readerId, startDate, endDate, isRegistered);

            if (items == null || items.Count == 0)
            {
                return NotFound("No items found for the specified reader in the given date range.");
            }

            return Ok(items);
        }

        [HttpGet("loanedItemsByShelf7")]
        public async Task<IActionResult> GetLoanedItemsByShelfAsync(Guid shelfId)
        {
            if (shelfId == Guid.Empty)
            {
                return BadRequest("Shelf ID is required.");
            }

            var loanedItems = await _readerService.GetLoanedItemsByShelfAsync7(shelfId);

            if (loanedItems == null || loanedItems.Count == 0)
            {
                return NotFound("No loaned items found for the specified shelf.");
            }

            return Ok(loanedItems);
        }

        [HttpGet("readersServicedByLibrarian8")]
        public async Task<IActionResult> GetReadersServicedByLibrarianAsync(Guid librarianId, DateTime startDate, DateTime endDate)
        {
            if (librarianId == Guid.Empty)
            {
                return BadRequest("Librarian ID is required.");
            }

            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            var readers = await _readerService.GetReadersServicedByLibrarianAsync8(librarianId, startDate, endDate);

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers found serviced by the specified librarian in the given date range.");
            }

            return Ok(readers);
        }

        [HttpGet("librarianWorkReport9")]
        public async Task<IActionResult> GetLibrarianWorkReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            var reports = await _readerService.GetLibrarianWorkReportAsync9(startDate, endDate);

            if (reports == null || reports.Count == 0)
            {
                return NotFound("No librarian work reports found for the specified date range.");
            }

            return Ok(reports);
        }

        [HttpGet("readersWithOverdueLoans10")]
        public async Task<IActionResult> GetReadersWithOverdueLoansAsync()
        {
            var readers = await _readerService.GetReadersWithOverdueLoansAsync10();

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers with overdue loans found.");
            }

            return Ok(readers);
        }

        [HttpGet("itemCopies11")]
        public async Task<IActionResult> GetItemCopiesReceivedOrWithdrawnWithDetailsAsync(
            DateTime startDate,
            DateTime endDate,
            bool includeReceived = true,
            bool includeWithdrawn = false)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than end date.");
            }

            var itemCopies = await _readerService.GetItemCopiesReceivedOrWithdrawnWithDetailsAsync11(
                startDate, endDate, includeReceived, includeWithdrawn);

            if (itemCopies == null || itemCopies.Count == 0)
            {
                return NotFound("No item copies found for the specified date range.");
            }

            return Ok(itemCopies);
        }

        [HttpGet("librarians12")]
        public async Task<IActionResult> GetLibrariansByReadingRoomIdAsync(Guid readingRoomId)
        {
            var librarians = await _readerService.GetLibrariansByReadingRoomIdAsync12(readingRoomId);

            if (librarians == null || librarians.Count == 0)
            {
                return NotFound("No librarians found for the specified reading room ID.");
            }

            return Ok(librarians);
        }

        [HttpGet("notVisited13")]
        public async Task<IActionResult> GetReadersNotVisitedAsync(DateTime sinceDate, DateTime toDate)
        {
            var readers = await _readerService.GetReadersNotVisitedAsync13(sinceDate, toDate);

            if (readers == null || readers.Count == 0)
            {
                return NotFound("No readers found who have not visited since the specified date.");
            }

            return Ok(readers);
        }

        [HttpGet("itemCopies14")]
        public async Task<IActionResult> GetItemCopiesByPublicationAsync(string publication)
        {
            if (string.IsNullOrWhiteSpace(publication))
            {
                return BadRequest("Publication name cannot be null or empty.");
            }

            var itemCopies = await _readerService.GetItemCopiesByPublicationAsync14(publication);

            if (itemCopies == null || itemCopies.Count == 0)
            {
                return NotFound("No item copies found for the specified publication.");
            }

            return Ok(itemCopies);
        }

        [HttpPost("searchItems15")]
        public async Task<IActionResult> SearchItemsAsync([FromBody] List<Guid>? authorIds)
        {
            var items = await _readerService.SearchItemsAsync15(authorIds);

            if (items == null || items.Count == 0)
            {
                return NotFound("No items found for the provided author IDs.");
            }

            return Ok(items);
        }

        [HttpGet("popularPublications16")]
        public async Task<IActionResult> GetMostPopularPublicationsAsync()
        {
            var popularPublications = await _readerService.GetMostPopularPublicationsAsync16();

            if (popularPublications == null || popularPublications.Count == 0)
            {
                return NotFound("No popular publications found.");
            }

            return Ok(popularPublications);
        }

        [HttpGet("shelf-by-id/{libraryId}")]
        public async Task<ActionResult<List<ShelfDto>>> GetShelvesByLibraryId(Guid libraryId)
        {
            if (libraryId == Guid.Empty)
            {
                return BadRequest("Library ID is required.");
            }

            var shelves = await _readerService.GetShelvesByLibraryIdAsync(libraryId);

            if (shelves == null || shelves.Count == 0)
            {
                return NotFound("No shelves found for the specified library.");
            }

            return Ok(shelves);
        }

        [HttpGet("reading-room-by-id/{libraryId}")]
        public async Task<ActionResult<List<ShelfDto>>> GetReadingRoomsByLibraryIdAsync(Guid libraryId)
        {
            if (libraryId == Guid.Empty)
            {
                return BadRequest("Library ID is required.");
            }

            var readinfRooms = await _readerService.GetReadingRoomsByLibraryIdAsync(libraryId);

            if (readinfRooms == null || readinfRooms.Count == 0)
            {
                return NotFound("No shelves found for the specified library.");
            }

            return Ok(readinfRooms);
        }
    }
}
