using Library.Core.Abstaction;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class ItemCopyService : IItemCopyService
    {
        private readonly IItemCopyRepository _itemCopyRepository;

        public ItemCopyService(IItemCopyRepository itemCopyRepository)
        {
            _itemCopyRepository = itemCopyRepository;
        }

        // Получение копии предмета по ID
        public async Task<ItemCopy?> GetByIdAsync(Guid itemCopyId)
        {
            return await _itemCopyRepository.GetByIdAsync(itemCopyId);
        }

        // Получение всех копий предметов
        public async Task<List<ItemCopy>> GetAllAsync()
        {
            return await _itemCopyRepository.GetAllAsync();
        }

        // Добавление новой копии предмета
        public async Task<Guid> AddAsync(ItemCopy itemCopy)
        {
            return await _itemCopyRepository.AddAsync(itemCopy);
        }

        // Обновление копии предмета
        public async Task<Guid> UpdateAsync(ItemCopy itemCopy)
        {
            return await _itemCopyRepository.UpdateAsync(itemCopy);
        }

        // Удаление копии предмета по ID
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _itemCopyRepository.DeleteAsync(id);
        }

        // Поиск копий предметов по параметрам
        public async Task<List<ItemCopy>> SearchItemCopiesAsync(string? inventoryNumber, Guid? itemId, Guid? shelfId)
        {
            return await _itemCopyRepository.SearchItemCopiesAsync(inventoryNumber, itemId, shelfId);
        }

        public async Task<Guid> ReportLoan(Guid itemCopyId)
        {
            return await _itemCopyRepository.ReportLoan(itemCopyId);
        }

        public async Task<Guid> CancelLoan(Guid itemCopyId)
        {
            return await _itemCopyRepository.CancelLoan(itemCopyId);
        }

        public async Task<Guid> ReportLostAsync(Guid itemCopyId)
        {
            return await _itemCopyRepository.ReportLostAsync(itemCopyId);
        }

        public async Task<Guid> CancelLostAsync(Guid itemCopyId)
        {
            return await _itemCopyRepository.CancelLostAsync(itemCopyId);
        }

        public async Task<Guid> WithdrawnItemCopy(Guid id)
        {
            return await _itemCopyRepository.WithdrawnItemCopy(id);
        }
    }
}
