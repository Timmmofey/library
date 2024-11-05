using Library.Core.Abstaction;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class ReadingRoomService : IReadingRoomService // Обновлено: реализация интерфейса
    {
        private readonly IReadingRoomRepository _readingRoomRepository;

        public ReadingRoomService(IReadingRoomRepository readingRoomRepository)
        {
            _readingRoomRepository = readingRoomRepository;
        }

        public async Task<ReadingRoom> GetByIdAsync(Guid id)
        {
            return await _readingRoomRepository.GetByIdAsync(id);
        }

        public async Task<List<ReadingRoom>> GetAllAsync()
        {
            return await _readingRoomRepository.GetAllAsync();
        }

        public async Task<Guid> AddAsync(ReadingRoom readingRoom)
        {
            return await _readingRoomRepository.AddAsync(readingRoom);
        }

        public async Task<Guid> UpdateAsync(ReadingRoom readingRoom)
        {
            return await _readingRoomRepository.UpdateAsync(readingRoom);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _readingRoomRepository.DeleteAsync(id);
        }

        public async Task<List<ReadingRoom>> SearchReadingRoomsAsync(string? name)
        {
            return await _readingRoomRepository.SearchReadingRoomsAsync(name);
        }
    }
}
