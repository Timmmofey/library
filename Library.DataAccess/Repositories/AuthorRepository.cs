using Library.Core.Abstaction;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Author?> GetByIdAsync(Guid authorId)
        {
            var authorEntity = await _context.Authors
                .AsNoTracking()
                .Include(a => a.Items)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if (authorEntity == null)
            {
                return null;
            }

            var items = authorEntity.Items.Select(itemEntity =>
                Item.Create(
                    itemEntity.Id,
                    itemEntity.Title,
                    itemEntity.CategoryId,
                    itemEntity.Publications,
                    itemEntity.Authors.Select(author => Author.Create(author.Id, author.Name, author.Description).Author).ToList()
                ).Item
            ).ToList();

            var author = Author.Create(
                authorEntity.Id,
                authorEntity.Name,
                authorEntity.Description,
                items
            ).Author;

            return author;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            var authorEntities = await _context.Authors
                .AsNoTracking()
                .Include(a => a.Items) 
                .ToListAsync();

            var authors = new List<Author>();

            foreach (var authorEntity in authorEntities)
            {
                var items = authorEntity.Items.Select(itemEntity =>
                    Item.Create(
                        itemEntity.Id,
                        itemEntity.Title,
                        itemEntity.CategoryId,
                        itemEntity.Publications,
                        itemEntity.Authors.Select(author => Author.Create(author.Id, author.Name, author.Description).Author).ToList()
                    ).Item
                ).ToList();

                var author = Author.Create(
                    authorEntity.Id,
                    authorEntity.Name,
                    authorEntity.Description,
                    items
                ).Author;

                authors.Add(author);
            }

            return authors;
        }

        public async Task<Guid> AddAsync(Author author)
        {
            var authorEntity = new AuthorEntity
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
            };

            await _context.Authors.AddAsync(authorEntity);
            await _context.SaveChangesAsync();

            return author.Id;
        }

        public async Task<Guid> UpdateAsync(Author author)
        {
            var authorEntity = await _context.Authors.FindAsync(author.Id);
            if (authorEntity == null) throw new KeyNotFoundException("Author not found.");

            authorEntity.Name = author.Name;
            authorEntity.Description = author.Description;

            await _context.SaveChangesAsync();

            return author.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var authorEntity = await _context.Authors.FindAsync(id);
            if (authorEntity == null) throw new KeyNotFoundException("Author not found.");

            _context.Authors.Remove(authorEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<List<Author>> SearchAuthorsAsync(string? name)
        {
            var query = _context.Authors
                .AsNoTracking()
                .Include(a => a.Items)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
            }

            var authorEntities = await query.ToListAsync();

            var authors = new List<Author>();

            foreach (var authorEntity in authorEntities)
            {
                var items = authorEntity.Items.Select(itemEntity =>
                    Item.Create(
                        itemEntity.Id,
                        itemEntity.Title,
                        itemEntity.CategoryId,
                        itemEntity.Publications,
                        itemEntity.Authors.Select(author => Author.Create(author.Id, author.Name, author.Description).Author).ToList()
                    ).Item
                ).ToList();

                var author = Author.Create(
                    authorEntity.Id,
                    authorEntity.Name,
                    authorEntity.Description,
                    items
                ).Author;

                authors.Add(author);
            }

            return authors;
        }


    }
}
