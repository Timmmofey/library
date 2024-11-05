using Library.API.Controllers.Contracts;
using Library.Application.Services;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReadingRoomController : ControllerBase
    {
        private readonly IReadingRoomService _readingRoomService;
        private readonly ILibrarianService _librarianService;

        public ReadingRoomController(IReadingRoomService readingRoomService, ILibrarianService librarianService)
        {
            _readingRoomService = readingRoomService;
            _librarianService = librarianService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadingRoomResponse>> GetById(Guid id)
        {
            var readingRoom = await _readingRoomService.GetByIdAsync(id);
            if (readingRoom == null)
            {
                return NotFound();
            }

            var response = new ReadingRoomResponse
            {
                Id = readingRoom.Id,
                Name = readingRoom.Name,
                LibraryId = readingRoom.LibraryId,
                Librarians = readingRoom.Librarians.Select(l => new LibrarianResponse { Id = l.Id, Name = l.Name, ReadingRoomId = l.ReadingRoomId }).ToList(),
                Sections = readingRoom.Sections.Select(s => new SectionResponse { Id = s.Id, Name = s.Name, ReadingRoomId = s.ReadingRoomId }).ToList()

            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ReadingRoomResponse>>> GetAll()
        {
            var readingRooms = await _readingRoomService.GetAllAsync();
            var response = readingRooms.Select(r => new ReadingRoomResponse
            {
                Id = r.Id,
                Name = r.Name,
                LibraryId = r.LibraryId,
                Librarians = r.Librarians.Select(l => new LibrarianResponse { Id = l.Id, Name = l.Name, ReadingRoomId = l.ReadingRoomId }).ToList(),
                Sections = r.Sections.Select(s => new SectionResponse { Id = s.Id, Name = s.Name, ReadingRoomId = s.ReadingRoomId}).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ReadingRoomResponse>> Add([FromBody] ReadingRoomRequest request)
        {
            var librarianId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(librarianId) || !Guid.TryParse(librarianId, out var librarianGuid))
            {
                return Unauthorized();
            }

            Librarian librarian = await _librarianService.GetByIdAsync(librarianGuid);

            Guid libraryID = librarian.LibraryId ?? throw new InvalidOperationException("Library ID is not set for the librarian.");

            //var (readingRoom, error) = ReadingRoom.Create(Guid.NewGuid(), request.Name, request.LibraryId);

            var (readingRoom, error) = ReadingRoom.Create(Guid.NewGuid(), request.Name, libraryID);


            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _readingRoomService.AddAsync(readingRoom);
            var response = new ReadingRoomResponse
            {
                Id = id,
                Name = request.Name,
                LibraryId = libraryID,
            };

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        //[HttpPut("{id:guid}")]
        //public async Task<ActionResult<ReadingRoomResponse>> Update(Guid id, [FromBody] ReadingRoomRequest request)
        //{
        //    var (readingRoom, error) = ReadingRoom.Create(id, request.Name, new List<Librarian>());

        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        return BadRequest(error);
        //    }

        //    var updatedReadingRoom = await _readingRoomService.UpdateAsync(readingRoom);
            
        //    return Ok(updatedReadingRoom);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    await _readingRoomService.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
