using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfController : ControllerBase
    {
        private readonly IShelfService _shelfService;

        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShelfResponse>> GetByIdAsync(Guid id)
        {
            var shelf = await _shelfService.GetShelfByIdAsync(id);
            if (shelf == null)
            {
                return NotFound();
            }

            var response = new ShelfResponse
            {
                Id = shelf.Id,
                SectionId = shelf.SectionId,
                Number = shelf.Number,
                ItemCopies = shelf.ItemCopies.Select(i => new ItemCopyResponse 
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ShelfId = i.ShelfId,
                    InventoryNumber = i.InventoryNumber,
                    Loanable = i.Loanable,
                    DateReceived = i.DateReceived,
                    DateWithdrawn = i.DateWithdrawn
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ShelfResponse>>> GetAllAsync()
        {
            var shelves = await _shelfService.GetAllShelvesAsync();
            var responses = shelves.Select(shelf => new ShelfResponse
            {
                Id = shelf.Id,
                SectionId = shelf.SectionId,
                Number = shelf.Number,
                ItemCopies = shelf.ItemCopies.Select(i => new ItemCopyResponse
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ShelfId = i.ShelfId,
                    InventoryNumber = i.InventoryNumber,
                    Loanable = i.Loanable,
                    DateReceived = i.DateReceived,
                    DateWithdrawn = i.DateWithdrawn
                }).ToList()
            }).ToList();

            return Ok(responses);
        }

        [HttpPost]
        public async Task<ActionResult<ShelfResponse>> AddAsync([FromBody] ShelfRequest request)
        {
            var shelf = Shelf.Create(Guid.NewGuid(), request.SectionId, request.Number).Shelf;
            var shelfId = await _shelfService.AddShelfAsync(shelf);

            var response = new ShelfResponse
            {
                Id = shelfId,
                SectionId = request.SectionId,
                Number = request.Number
            };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = shelfId }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShelfResponse>> UpdateAsync(Guid id, [FromBody] ShelfRequest request)
        {
            var shelf = Shelf.Create(id, request.SectionId, request.Number).Shelf;
            await _shelfService.UpdateShelfAsync(shelf);

            var response = new ShelfResponse
            {
                Id = shelf.Id,
                SectionId = shelf.SectionId,
                Number = shelf.Number
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _shelfService.DeleteShelfAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ShelfResponse>>> SearchAsync(string? number)
        {
            var shelves = await _shelfService.SearchShelvesAsync(number);
            var responses = shelves.Select(shelf => new ShelfResponse
            {
                Id = shelf.Id,
                SectionId = shelf.SectionId,
                Number = shelf.Number,
                ItemCopies = shelf.ItemCopies.Select(i => new ItemCopyResponse
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ShelfId = i.ShelfId,
                    InventoryNumber = i.InventoryNumber,
                    Loanable = i.Loanable,
                    DateReceived = i.DateReceived,
                    DateWithdrawn = i.DateWithdrawn
                }).ToList()
            }).ToList();

            return Ok(responses);
        }
    }
}