using Library.API.Controllers.Contracts;
using Library.Core.Abstaction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponse>> GetItemById(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var response = new ItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                CategoryId = item.CategoryId,
                PublicationDate = item.Publications,
                Authors = item.Authors.Select(author => new AuthorResponse
                {
                    Id = author.Id,
                    Name = author.Name,
                    Description = author.Description,
                    Items = author.Items.Select(item => new ItemResponse
                    {
                        Id = item.Id,
                        Title = item.Title,
                        CategoryId = item.CategoryId,
                        PublicationDate = item.Publications
                    }).ToList()
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemResponse>>> GetAllItems()
        {
            var items = await _itemService.GetAllAsync();
            var response = items.Select(item => new ItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                CategoryId = item.CategoryId,
                PublicationDate = item.Publications,
                Authors = item.Authors.Select(author => new AuthorResponse
                {
                    Id = author.Id,
                    Name = author.Name,
                    Description = author.Description,
                    Items = author.Items.Select(item => new ItemResponse
                    {
                        Id = item.Id,
                        Title = item.Title,
                        CategoryId = item.CategoryId,
                        PublicationDate = item.Publications
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateItem(ItemRequest request)
        {
            var authors = new List<Author>();
            foreach (var authorId in request.AuthorIds)
            {
                var (author, createError) = Author.Create(authorId, "Default Name", "Default Description");
                if (!string.IsNullOrEmpty(createError))
                {
                    return BadRequest(createError);
                }
                authors.Add(author);
            }

            var (item, error) = Item.Create(
                Guid.NewGuid(),
                request.Title,
                request.CategoryId,
                request.PublicationDate,
                authors
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _itemService.AddAsync(item);
            return CreatedAtAction(nameof(GetItemById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, ItemRequest request)
        {
            var authors = new List<Author>();

            foreach (var authorId in request.AuthorIds)
            {
                var (author, createError) = Author.Create(authorId, "Default Name", "Default Description");
                if (!string.IsNullOrEmpty(createError))
                {
                    return BadRequest(createError);
                }
                authors.Add(author);
            }

            var (item, itemError) = Item.Create(
                id,
                request.Title,
                request.CategoryId,
                request.PublicationDate,
                authors
            );

            if (!string.IsNullOrEmpty(itemError))
            {
                return BadRequest(itemError);
            }

            await _itemService.UpdateAsync(item);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            await _itemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ItemResponse>>> SearchItems([FromQuery] ItemSearchRequest request)
        {
            var items = await _itemService.SearchItemsAsync(
                request.Title,
                request.CategoryId,
                request.PublicationDate,
                request.AuthorIds
            );

            var itemResponses = items.Select(item => new ItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                CategoryId = item.CategoryId,
                PublicationDate = item.Publications,
                Authors = item.Authors.Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                }).ToList()
            }).ToList();

            return Ok(itemResponses);
        }
    }
}
