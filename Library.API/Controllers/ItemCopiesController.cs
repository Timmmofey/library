using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemCopiesController : ControllerBase
    {
        private readonly IItemCopyService _itemCopyService;

        public ItemCopiesController(IItemCopyService itemCopyService)
        {
            _itemCopyService = itemCopyService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCopyResponse>> GetItemCopyById(Guid id)
        {
            var itemCopy = await _itemCopyService.GetByIdAsync(id);

            if (itemCopy == null)
            {
                return NotFound();
            }

            var response = new ItemCopyResponse
            {
                Id = itemCopy.Id,
                ItemId = itemCopy.ItemId,
                ShelfId = itemCopy.ShelfId,
                InventoryNumber = itemCopy.InventoryNumber,
                Loanable = itemCopy.Loanable,
                Loaned = itemCopy.Loaned,
                DateReceived = itemCopy.DateReceived,
                DateWithdrawn = itemCopy.DateWithdrawn,
                Lost = itemCopy.Lost // Добавил поле Lost в ответ
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemCopyResponse>>> GetAllItemCopies()
        {
            var itemCopies = await _itemCopyService.GetAllAsync();

            var responses = itemCopies.Select(itemCopy => new ItemCopyResponse
            {
                Id = itemCopy.Id,
                ItemId = itemCopy.ItemId,
                ShelfId = itemCopy.ShelfId,
                InventoryNumber = itemCopy.InventoryNumber,
                Loanable = itemCopy.Loanable,
                Loaned = itemCopy.Loaned,
                DateReceived = itemCopy.DateReceived,
                DateWithdrawn = itemCopy.DateWithdrawn,
                Lost = itemCopy.Lost // Добавил поле Lost в ответ
            }).ToList();

            return Ok(responses);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateItemCopy(ItemCopyRequest request)
        {
            var (itemCopy, error) = ItemCopy.Create(
                Guid.NewGuid(),
                request.ItemId,
                request.ShelfId,
                request.InventoryNumber,
                request.Loanable,
                false,
                false, // Добавил поле Lost при создании
                DateTime.UtcNow.Date,
                null
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _itemCopyService.AddAsync(itemCopy);
            return CreatedAtAction(nameof(GetItemCopyById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemCopy(Guid id, ItemCopyRequest request)
        {
            var (itemCopy, error) = ItemCopy.Create(
                id,
                request.ItemId,
                request.ShelfId,
                request.InventoryNumber,
                request.Loanable
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            await _itemCopyService.UpdateAsync(itemCopy);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemCopy(Guid id)
        {
            await _itemCopyService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ItemCopyResponse>>> SearchItemCopies(
            [FromQuery] string? inventoryNumber,
            [FromQuery] Guid? itemId,
            [FromQuery] Guid? shelfId)
        {
            var itemCopies = await _itemCopyService.SearchItemCopiesAsync(inventoryNumber, itemId, shelfId);

            var responses = itemCopies.Select(itemCopy => new ItemCopyResponse
            {
                Id = itemCopy.Id,
                ItemId = itemCopy.ItemId,
                ShelfId = itemCopy.ShelfId,
                InventoryNumber = itemCopy.InventoryNumber,
                Loanable = itemCopy.Loanable,
                Loaned = itemCopy.Loaned,
                DateReceived = itemCopy.DateReceived,
                DateWithdrawn = itemCopy.DateWithdrawn,
                Lost = itemCopy.Lost // Добавил поле Lost в ответ
            }).ToList();

            return Ok(responses);
        }

        // Отчет о потере книги
        [HttpPost("{id}/report-lost")]
        public async Task<ActionResult<Guid>> ReportItemCopyLost(Guid id)
        {
            var result = await _itemCopyService.ReportLostAsync(id);
            return Ok(result);
        }

        // Отмена статуса потери книги
        [HttpPost("{id}/cancel-lost")]
        public async Task<ActionResult<Guid>> CancelItemCopyLost(Guid id)
        {
            var result = await _itemCopyService.CancelLostAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/withdrawn-item-copy")]
        public async Task<ActionResult<Guid>> WithdrawnItemCopy(Guid id)
        {
            var result = await _itemCopyService.WithdrawnItemCopy(id);
            return Ok(result);
        }
    }
}
