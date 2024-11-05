using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderCategoryController : ControllerBase
    {
        private readonly IReaderCategoryService _readerCategoryService;

        public ReaderCategoryController(IReaderCategoryService readerCategoryService)
        {
            _readerCategoryService = readerCategoryService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReaderCategoryResponse>> GetById(Guid id)
        {
            var category = await _readerCategoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new ReaderCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ReaderCategoryResponse>>> GetAll()
        {
            var categories = await _readerCategoryService.GetAllAsync();

            var response = categories.Select(c => new ReaderCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ReaderCategoryRequest request)
        {
            var category = ReaderCategory.Create(Guid.NewGuid(), request.Name, request.Description).Category; // Создание новой категории

            if (category == null)
            {
                return BadRequest("Invalid data.");
            }

            var categoryId = await _readerCategoryService.AddAsync(category.Name, category.Description);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, categoryId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ReaderCategoryRequest request)
        {
            var category = ReaderCategory.Create(id, request.Name, request.Description).Category; 

            if (category == null)
            {
                return BadRequest("Invalid data.");
            }

            await _readerCategoryService.UpdateAsync(category.Id, category.Name, category.Description);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _readerCategoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
