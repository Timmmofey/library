using Library.Core.Abstaction;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository _shelfRepository;

        public ShelfService(IShelfRepository shelfRepository)
        {
            _shelfRepository = shelfRepository;
        }

        public async Task<Shelf?> GetShelfByIdAsync(Guid shelfId)
        {
            return await _shelfRepository.GetByIdAsync(shelfId);
        }

        public async Task<List<Shelf>> GetAllShelvesAsync()
        {
            return await _shelfRepository.GetAllAsync();
        }

        public async Task<Guid> AddShelfAsync(Shelf shelf)
        {
            return await _shelfRepository.AddAsync(shelf);
        }

        public async Task<Guid> UpdateShelfAsync(Shelf shelf)
        {
            return await _shelfRepository.UpdateAsync(shelf);
        }

        public async Task<Guid> DeleteShelfAsync(Guid shelfId)
        {
            return await _shelfRepository.DeleteAsync(shelfId);
        }

        public async Task<List<Shelf>> SearchShelvesAsync(string? number)
        {
            return await _shelfRepository.SearchShelfsAsync(number);
        }
    }
}
