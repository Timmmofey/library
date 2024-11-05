using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryService _itemCategoryService;

        public ItemCategoryController(IItemCategoryService itemCategoryService)
        {
            _itemCategoryService = itemCategoryService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemCategoryResponse>> GetById(Guid id)
        {
            var category = await _itemCategoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new ItemCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemCategoryResponse>>> GetAll()
        {
            var categories = await _itemCategoryService.GetAllAsync();

            var response = categories.Select(c => new ItemCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ItemCategoryRequest request)
        {
            var categoryId = await _itemCategoryService.AddAsync(request.Name, request.Description);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, categoryId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ItemCategoryRequest request)
        {
            var existingCategory = await _itemCategoryService.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            var updatedCategory = ItemCategory.Create(id, request.Name, request.Description).Category;
            await _itemCategoryService.UpdateAsync(updatedCategory);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingCategory = await _itemCategoryService.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _itemCategoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}