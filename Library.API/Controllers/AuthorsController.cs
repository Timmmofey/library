using Library.API.Controllers.Contracts;
using Library.Core.Abstraction;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponse>> GetAuthorById(Guid id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            var response = new AuthorResponse
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
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorResponse>>> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAsync();

            var response = authors.Select(author => new AuthorResponse
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
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAuthor(AuthorRequest request)
        {
            var (author, error) = Author.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                new List<Item>()
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _authorService.AddAsync(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(Guid id, AuthorRequest request)
        {
            var (author, error) = Author.Create(
                id,
                request.Name,
                request.Description,
                new List<Item>()
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            try
            {
                await _authorService.UpdateAsync(author);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); 
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                await _authorService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); 
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AuthorResponse>>> SearchAuthors([FromQuery] string? name)
        {
            var authors = await _authorService.SearchAuthorsAsync(name);

            var response = authors.Select(author => new AuthorResponse
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
            }).ToList();

            return Ok(response);
        }
    }
}
