using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        private readonly ILibrarianService _librarianService;


        public SectionController(ISectionService sectionService, ILibrarianService librarianService)
        {
            _sectionService = sectionService;
            _librarianService = librarianService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SectionResponse>> GetById(Guid id)
        {
            var section = await _sectionService.GetByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            var response = new SectionResponse
            {
                Id = section.Id,
                ReadingRoomId = section.ReadingRoomId,
                Name = section.Name,
                Shelves = section.Shelves.Select(s => new ShelveResponse
                {
                    Id = s.Id,
                    SectionId = s.SectionId,
                    Number = s.Number
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<SectionResponse>>> GetAll()
        {
            var sections = await _sectionService.GetAllAsync();

            var response = sections.Select(s => new SectionResponse
            {
                Id = s.Id,
                ReadingRoomId = s.ReadingRoomId,
                Name = s.Name,
                Shelves = s.Shelves.Select(sh => new ShelveResponse
                {
                    Id = sh.Id,
                    SectionId = sh.SectionId,
                    Number = sh.Number
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        //[HttpPost]
        //public async Task<ActionResult<Guid>> Create([FromBody] SectionRequest request)
        //{
        //    var section = Section.Create(Guid.NewGuid(), request.ReadingRoomId, request.Name).Section;

        //    if (section == null)
        //    {
        //        return BadRequest("Invalid data.");
        //    }

        //    var sectionId = await _sectionService.AddAsync(section);
        //    return CreatedAtAction(nameof(GetById), new { id = sectionId }, sectionId);
        //}

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] SectionRequest request)
        {
            var section = Section.Create(Guid.NewGuid(), request.ReadingRoomId, request.Name).Section;

            if (section == null)
            {
                return BadRequest("Invalid data.");
            }

            var sectionId = await _sectionService.AddAsync(section);
            return CreatedAtAction(nameof(GetById), new { id = sectionId }, sectionId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] SectionRequest request)
        {
            var section = Section.Create(id, request.ReadingRoomId, request.Name).Section;

            if (section == null)
            {
                return BadRequest("Invalid data.");
            }

            await _sectionService.UpdateAsync(section);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _sectionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<SectionResponse>>> Search([FromQuery] string name)
        {
            var sections = await _sectionService.SearchSectionsAsync(name);

            var response = sections.Select(s => new SectionResponse
            {
                Id = s.Id,
                ReadingRoomId = s.ReadingRoomId,
                Name = s.Name,
                Shelves = s.Shelves.Select(sh => new ShelveResponse
                {
                    Id = sh.Id,
                    SectionId = sh.SectionId,
                    Number = sh.Number
                }).ToList()
            }).ToList();

            return Ok(response);
        }
    }
}
