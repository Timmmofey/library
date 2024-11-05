using Library.Core.Models;
using Library.Controllers.Contracts;
using Library.Core.Abstaction;
using Microsoft.AspNetCore.Mvc;
using Library.API.Controllers.Contracts;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
             
        [HttpGet]
        public async Task<ActionResult<List<LibraryResponseModel>>> GetLibraries()
        {
            var libraries = await _libraryService.GetAllLibrariesAsync();

            var response = libraries.Select(l => new LibraryResponseModel
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address,
                Description = l.Description,
                ReadingRooms = l.ReadingRooms.Select(s => new ReadingRoomResponse { Id = s.Id, Name = s.Name }).ToList(),
                Readers = l.Readers.Select(r => new ReaderResponse { Id = r.Id, FullName = r.FullName, LibraryId = r.LibraryId }).ToList(),
            }).ToList();

            return Ok(response);
        }


        [HttpPost]
        
        public async Task<ActionResult<Guid>> CreateLibrary([FromBody] LibraryRequestModel request)
        {
            var (library, error) = LibraryModel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Address,
                request.Description,
                new List<ReadingRoom>(),
                new List<Reader>()
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var libraryId = await _libraryService.AddLibraryAsync(library);
            return Ok(libraryId);
        }

        [HttpPut("{id:guid}")]
        
        public async Task<ActionResult<Guid>> UpdateLibrary(Guid id, [FromBody] LibraryRequestModel request)
        {
            var (library, error) = LibraryModel.Create(
                id,
                request.Name,
                request.Address,
                request.Description,
                new List<ReadingRoom>(), 
                new List<Reader>()
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var updatedLibraryId = await _libraryService.UpdateLibraryAsync(library);
            return Ok(updatedLibraryId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteLibrary(Guid id)
        {
            await _libraryService.DeleteLibraryAsync(id);
            return Ok(id);
        }
               
        [HttpGet("search")]
        public async Task<ActionResult<List<LibraryResponseModel>>> SearchLibraries([FromQuery] string? name, [FromQuery] string? address, [FromQuery] string? description)
        {
            var libraries = await _libraryService.SearchLibrariesAsync(name, address, description);

            var response = libraries.Select(l => new LibraryResponseModel
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address,
                Description = l.Description,
                ReadingRooms = l.ReadingRooms.Select(s => new ReadingRoomResponse { Id = s.Id, Name = s.Name }).ToList(),
                Readers = l.Readers.Select(r => new ReaderResponse { Id = r.Id, FullName = r.FullName,  LibraryId = r.LibraryId}).ToList(),
            }).ToList();

            return Ok(response);
        }

    }
}
