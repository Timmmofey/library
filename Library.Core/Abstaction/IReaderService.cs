using Library.Core.DTOs;
using Library.Core.Models;

namespace Library.Core.Abstaction
{
    public interface IReaderService
    {
        Task<Reader> GetByIdAsync(Guid id);
        Task<List<Reader>> GetAllAsync();
        Task<Guid> Register(Reader reader);
        Task<Guid> UpdateAsync(Reader reader);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<Reader>> SearchReadersAsync(
            string? fullName,
            Guid? libraryId,
            Guid? ReaderCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic
            );
        Task<string> Login(string email, string password);

        Task<Guid> ExtendSubscription(Guid id, int days);

        Task<Guid> EditAdditionalInfo(Guid id,
                       Guid? readerCategoryId,
                       string? educationalInstitution,
                       string? faculty,
                       string? course,
                       string? groupNumber,
                       string? organization,
                       string? researchTopic);

        Task<Guid> EditMainInfo(Guid id, string? email, string? passwordHash, string? fullName, Guid? libraryId);

        Task<List<ReaderDto1>> SearchReadersAsync1(
            string? fullName,
            Guid? libraryId,
            Guid? ReaderCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic
            );

        Task<List<Reader>> GetReadersWithBookAsync2(string publication);
        Task<List<ReaderDto1>> GetReadersWithItemAsync3(Guid itemId);
        Task<List<ReaderWithItemDto>> GetReadersWithItemInDateRangeAsync4(Guid itemId, DateTime startDate, DateTime endDate);
        Task<List<ItemCopyDto1>> GetItemsByReaderAndRegistrationStatusAsync56(
                Guid readerId,
                DateTime startDate,
                DateTime endDate,
                bool isRegistered);
        Task<List<ItemCopyDto1>> GetLoanedItemsByShelfAsync7(Guid shelfId);
        Task<List<Reader>> GetReadersServicedByLibrarianAsync8(Guid librarianId, DateTime startDate, DateTime endDate);
        Task<List<LibrarianWorkReport>> GetLibrarianWorkReportAsync9(DateTime startDate, DateTime endDate);
        Task<List<ReaderDto1>> GetReadersWithOverdueLoansAsync10();
        Task<List<ItemCopyDto1>> GetItemCopiesReceivedOrWithdrawnWithDetailsAsync11(
            DateTime startDate,
            DateTime endDate,
            bool includeReceived = true,
            bool includeWithdrawn = false);
        Task<List<LibrarianDto1>> GetLibrariansByReadingRoomIdAsync12(Guid readingRoomId);
        Task<List<ReaderDto>> GetReadersNotVisitedAsync13(DateTime sinceDate, DateTime toDate);
        Task<List<ItemCopyDto1>> GetItemCopiesByPublicationAsync14(string publication);
        Task<List<ItemCopyDto1>> SearchItemsAsync15(List<Guid>? authorIds);
        Task<List<PopularPublicationDto>> GetMostPopularPublicationsAsync16();
        Task<List<ShelfDto>> GetShelvesByLibraryIdAsync(Guid libraryId);
        Task<List<ReadingRoomDto>> GetReadingRoomsByLibraryIdAsync(Guid libraryId);
    }
}

