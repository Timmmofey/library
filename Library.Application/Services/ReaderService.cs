using Library.Application.Interfaces.Auth;
using Library.Core.Abstaction;
using Library.Core.DTOs;
using Library.Core.Models;
using Library.Infrastructure;

namespace Library.Core.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public ReaderService(
            IReaderRepository readerRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider
            )
        {
            _readerRepository = readerRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Reader> GetByIdAsync(Guid id)
        {
            return await _readerRepository.GetByIdAsync(id);
        }

        public async Task<List<Reader>> GetAllAsync()
        {
            return await _readerRepository.GetAllAsync();
        }

        public async Task<Guid> Register(Reader readerReq)
        {
            if (readerReq == null)
            {
                throw new ArgumentNullException(nameof(readerReq), "Reader request cannot be null.");
            }

            var hashedPassword = _passwordHasher.Generate(readerReq.PasswordHash);

            var (reader, error) = Reader.Create(
                readerReq.Id,
                readerReq.Email,
                hashedPassword,
                readerReq.FullName,
                readerReq.LibraryId
            );

            if (reader == null)
            {
                throw new ArgumentException(error); // Или обработка ошибки, если нужно
            }

            return await _readerRepository.AddReader(reader);
        }

        public async Task<Guid> UpdateAsync(Reader reader)
        {
            return await _readerRepository.UpdateAsync(reader);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _readerRepository.DeleteAsync(id);
        }

        public async Task<List<Reader>> SearchReadersAsync(
            string? fullName,
            Guid? libraryId,
            Guid? ReaderCategoryId,
            string? educationalInstitution,
            string? faculty,
            string? course,
            string? groupNumber,
            string? organization,
            string? researchTopic
            )
        {
            return await _readerRepository.SearchReadersAsync(fullName, libraryId, ReaderCategoryId, educationalInstitution, faculty, course, groupNumber, organization, researchTopic);
        }

        public async Task<string> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

            var reader = await _readerRepository.GetByEmail(email);

            if (reader == null)
            {
                throw new Exception("User not found");
            }

            if (reader.PasswordHash == null)
            {
                throw new Exception("Password hash not found for user.");
            }

            var result = _passwordHasher.Verify(password, reader.PasswordHash);

            if (!result)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateReaderToken(reader);

            if (token == null)
            {
                throw new Exception("Failed to generate token");
            }

            return token;
        }

        public async Task<Guid> ExtendSubscription(Guid id, int days)
        {
            return await _readerRepository.ExtendSubscription(id, days);
        }

        public async Task<Guid> EditAdditionalInfo(Guid id,
                       Guid? readerCategoryId,
                       string? educationalInstitution,
                       string? faculty,
                       string? course,
                       string? groupNumber,
                       string? organization,
                       string? researchTopic)
        {

            return await _readerRepository.EditAdditionalInfo(id, readerCategoryId, educationalInstitution, faculty, course, groupNumber, organization, researchTopic);
        }


        public async Task<Guid> EditMainInfo(Guid id, string? email, string? passwordHash, string? fullName, Guid? libraryId)
        {
            var hashedPassword = passwordHash;

            if (passwordHash != null)
            {
                hashedPassword = _passwordHasher.Generate(passwordHash);
            }

            return await _readerRepository.EditMainInfo(id, email, hashedPassword, fullName, libraryId);
        }

        public async Task<List<ReaderDto1>> SearchReadersAsync1(
             string? fullName,
             Guid? libraryId,
             Guid? ReaderCategoryId,
             string? educationalInstitution,
             string? faculty,
             string? course,
             string? groupNumber,
             string? organization,
             string? researchTopic
             )
        {
            return await _readerRepository.SearchReadersAsync1(
                fullName,
                libraryId,
                ReaderCategoryId,
                educationalInstitution,
                faculty,
                course,
                groupNumber,
                organization,
                researchTopic);
        }

        public async Task<List<Reader>> GetReadersWithBookAsync2(string publication)
        {
            return await _readerRepository.GetReadersWithBookAsync2(publication);
        }

        public async Task<List<ReaderDto1>> GetReadersWithItemAsync3(Guid itemId)
        {
            return await _readerRepository.GetReadersWithItemAsync3(itemId);
        }

        public async Task<List<ReaderWithItemDto>> GetReadersWithItemInDateRangeAsync4(Guid itemId, DateTime startDate, DateTime endDate)
        {
            return await _readerRepository.GetReadersWithItemInDateRangeAsync4(itemId, startDate, endDate);
        }

        public async Task<List<ItemCopyDto1>> GetItemsByReaderAndRegistrationStatusAsync56(
                            Guid readerId,
                DateTime startDate,
                DateTime endDate,
                bool isRegistered)
        {
            return await _readerRepository.GetPublicationsByReaderAndLibraryRegistrationAsync56(readerId, startDate, endDate, isRegistered);
        }

        public async Task<List<ItemCopyDto1>> GetLoanedItemsByShelfAsync7(Guid shelfId)
        {
            return await _readerRepository.GetLoanedItemsByShelfAsync7(shelfId);
        }

        public async Task<List<Reader>> GetReadersServicedByLibrarianAsync8(Guid librarianId, DateTime startDate, DateTime endDate)
        {
            return await _readerRepository.GetReadersServicedByLibrarianAsync8(librarianId, startDate, endDate);    
        }

        public async Task<List<LibrarianWorkReport>> GetLibrarianWorkReportAsync9(DateTime startDate, DateTime endDate)
        {
            return await _readerRepository.GetLibrarianWorkReportAsync9(startDate, endDate);
        }

        public async Task<List<ReaderDto1>> GetReadersWithOverdueLoansAsync10()
        {
            return await _readerRepository.GetReadersWithOverdueLoansAsync10();
        }

        public async Task<List<ItemCopyDto1>> GetItemCopiesReceivedOrWithdrawnWithDetailsAsync11(
            DateTime startDate,
            DateTime endDate,
            bool includeReceived = true,
            bool includeWithdrawn = false)
        {
            return await _readerRepository.GetItemCopiesReceivedOrWithdrawnWithDetailsAsync11(startDate, endDate, includeReceived, includeWithdrawn);
        }

        public async Task<List<LibrarianDto1>> GetLibrariansByReadingRoomIdAsync12(Guid readingRoomId)
        {
            return await _readerRepository.GetLibrariansByReadingRoomIdAsync12(readingRoomId);
        }

        public async Task<List<ReaderDto>> GetReadersNotVisitedAsync13(DateTime sinceDate, DateTime toDate)
        {
            return await _readerRepository.GetReadersNotVisitedAsync13(sinceDate, toDate);
        }

        public async Task<List<ItemCopyDto1>> GetItemCopiesByPublicationAsync14(string publication)
        {
            return await _readerRepository.GetItemCopiesByPublicationAsync14(publication);
        }

        public async Task<List<ItemCopyDto1>> SearchItemsAsync15(List<Guid>? authorIds)
        {
            return await _readerRepository.SearchItemsAsync15(authorIds);
        }

        public async Task<List<PopularPublicationDto>> GetMostPopularPublicationsAsync16()
        {
            return await _readerRepository.GetMostPopularPublicationsAsync16();
        }

        public async Task<List<ShelfDto>> GetShelvesByLibraryIdAsync(Guid libraryId)
        {
            return await _readerRepository.GetShelvesByLibraryIdAsync(libraryId);
        }

        public async Task<List<ReadingRoomDto>> GetReadingRoomsByLibraryIdAsync(Guid libraryId)
        {
            return await _readerRepository.GetReadingRoomsByLibraryIdAsync(libraryId);
        }
    }
}
