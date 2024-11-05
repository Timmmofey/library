using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Library.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibrariansController : ControllerBase
    {
        private readonly ILibrarianService _librarianService;
        private readonly IReaderService _readerService;

        public LibrariansController(ILibrarianService librarianService, IReaderService readerService)
        {
            _librarianService = librarianService;
            _readerService = readerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibrarianResponse>> GetLibrarianById(Guid id)
        {
            var librarian = await _librarianService.GetByIdAsync(id);

            if (librarian == null)
            {
                return NotFound();
            }

            var response = new LibrarianResponse
            {
                Id = librarian.Id,
                Name = librarian.Name,
                Role = librarian.Role,
                Loans = librarian.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList(),
                ReadingRoomId = librarian.ReadingRoomId
            };

            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<LibrarianResponse>>> GetAllLibrarians()
        {
            var librarians = await _librarianService.GetAllAsync();
            var response = librarians.Select(librarian => new LibrarianResponse
            {
                Id = librarian.Id,
                Name = librarian.Name,
                ReadingRoomId = librarian.ReadingRoomId,
                Role = librarian.Role,
                Loans = librarian.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList()
            });

            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateEnity(LibrarianRequest request)
        {
            var (librarian, error) = Librarian.Create(
                Guid.NewGuid(),
                request.Name,
                request.Login,
                request.PasswordHash,
                request.Role,
                request.ReadingRoomId
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _librarianService.Register(librarian);
            return CreatedAtAction(nameof(GetLibrarianById), new { id }, id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("addDirector")]
        public async Task<ActionResult<Guid>> CreateDirector(LibrarianDirectorRequest request)
        {

            var (librarian, error) = Librarian.Create(
                Guid.NewGuid(),
                request.Name,
                request.Login,
                request.PasswordHash,
                Role.Director,
                request.LibraryId
            );

            try
            {
                var id = await _librarianService.AddDirector(librarian);
                return CreatedAtAction(nameof(GetLibrarianById), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("addLibrarian")]
        public async Task<ActionResult<Guid>> CreateLibrarian(LibrarianLibrarianRequest request)
        {
            var (librarian, error) = Librarian.Create(
                Guid.NewGuid(),
                request.Name,
                request.Login,
                request.PasswordHash,
                Role.Librarian,
                request.ReadingRoomId
            );

            try
            {
                var id = await _librarianService.AddLibrarian(librarian);
                return CreatedAtAction(nameof(GetLibrarianById), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLibrarian(Guid id, LibrarianRequest request)
        {
            var (librarian, error) = Librarian.Create(
                id,
                request.Name,
                request.Login,
                request.PasswordHash,
                request.Role,
                request.ReadingRoomId
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            await _librarianService.UpdateAsync(librarian);
            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLibrarian(Guid id)
        {
            await _librarianService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchLibrarians([FromQuery] string? name, [FromQuery] string? libraryName)
        {
            var librarians = await _librarianService.SearchLibrariansAsync(name, libraryName);

            if (librarians == null || !librarians.Any())
            {
                return NotFound("Библиотекари не найдены.");
            }

            return Ok(librarians);
        }

        [AllowAnonymous]
        [HttpPost("loginLibrarian")]
        public async Task<IActionResult> LoginLibrarian(LoginLibrarianRequest request)
        {
            try
            {
                var token = await _librarianService.Login(request.Login, request.Password);

                Response.Cookies.Append("some-cookie", token);

                return Ok(new
                {
                    Token = token,
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("editReaderAdditionalInfo")]
        public async Task<ActionResult<Guid>> editReaderAdditionalInfo(Guid id,
                       Guid? readerCategoryId,
                       string? educationalInstitution,
                       string? faculty,
                       string? course,
                       string? groupNumber,
                       string? organization,
                       string? researchTopic)
        {
            await _readerService.EditAdditionalInfo(id, readerCategoryId, educationalInstitution, faculty, course, groupNumber, organization, researchTopic);
            return Ok(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("editWorkerInfo")]
        public async Task<ActionResult<Guid>> editWorkerInfo(Guid id, string? name, string? login, string? passwordHash, Guid? libraryId, Guid? readingRoomId)
        {
            await _librarianService.EditWorkerInfo(id, name, login, passwordHash, libraryId, readingRoomId);
            return Ok(id);
        }

        [Authorize(Roles = "Librarian, Admin, Director")]
        [HttpPost("getWorkerInfo")]
        public async Task<ActionResult<ReaderResponse>> GetWorkerInfo()
        {
            var workerId = User.Claims.FirstOrDefault(r => r.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(workerId) || !Guid.TryParse(workerId, out var workerGuid))
            {
                return Unauthorized();
            }

            var librarian = await _librarianService.GetByIdAsync(workerGuid);

            if (librarian == null)
            {
                return NotFound();
            }

            var response = new LibrarianResponse
            {
                Id = librarian.Id,
                Name = librarian.Name,
                Role = librarian.Role,
                Loans = librarian.Loans.Select(loan => new LoanResponse
                {
                    Id = loan.Id,
                    ItemCopyId = loan.ItemCopyId,
                    ReaderId = loan.ReaderId,
                    LibrarianId = loan.LibrarianId,
                    IssueDate = loan.IssueDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate
                }).ToList(),
                ReadingRoomId = librarian.ReadingRoomId
            };

            return Ok(response);
        }

    }
}