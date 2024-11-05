using Library.Application.Interfaces.Auth;
using Library.Core.Abstaction;
using Library.Core.DTOs;
using Library.Core.Models;
using Library.Infrastructure;

namespace Library.Application.Services
{
    public class LibrariansService : ILibrarianService
    {
        private readonly ILibrarianRepository _librarianRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LibrariansService(
            ILibrarianRepository librarianRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider
        )
        {
            _librarianRepository = librarianRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Librarian> GetByIdAsync(Guid id)
        {
            return await _librarianRepository.GetByIdAsync(id);
        }

        public async Task<List<Librarian>> GetAllAsync()
        {
            return await _librarianRepository.GetAllAsync();
        }

        public async Task<Guid> Register(Librarian librarianReq)
        {
            var hashedPassword = _passwordHasher.Generate(librarianReq.PasswordHash);

            var (librarian, error) = Librarian.Create(
                librarianReq.Id,
                librarianReq.Name,
                librarianReq.Login,
                hashedPassword,
                librarianReq.Role, // Добавлено поле роли
                librarianReq.ReadingRoomId
            );

            if (librarian == null)
            {
                throw new ArgumentException(error); // Или обработка ошибки, если нужно
            }

            return await _librarianRepository.AddAsync(librarian);
        }

        public async Task<Guid> UpdateAsync(Librarian librarianReq)
        {
            // Генерация хешированного пароля
            var hashedPassword = _passwordHasher.Generate(librarianReq.PasswordHash);

            // Создание нового экземпляра Librarian с обновленными данными
            var (librarian, error) = Librarian.Create(
                librarianReq.Id,
                librarianReq.Name,
                librarianReq.Login,
                hashedPassword, // Используем хешированный пароль
                librarianReq.Role,// Новое поле роли
                librarianReq.ReadingRoomId
            );

            if (librarian == null)
            {
                throw new ArgumentException(error); // Обработка ошибки
            }

            // Обновление библиотекаря в репозитории
            return await _librarianRepository.UpdateAsync(librarian);
        }



        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _librarianRepository.DeleteAsync(id);
        }

        public async Task<List<LibrarianDto1>> SearchLibrariansAsync(string? name, string? libraryId)
        {
            return await _librarianRepository.SearchLibrariansAsync(name, libraryId);
        }

        //public async Task<string> Login(string login, string password)
        //{
        //    var librarian = await _librarianRepository.GetByLogin(login);

        //    if (librarian == null)
        //    {
        //        throw new Exception("User not found");
        //    }

        //    var result = _passwordHasher.Verify(password, librarian.PasswordHash);

        //    if (!result)
        //    {
        //        throw new Exception("Failed to login");
        //    }

        //    // Генерация JWT токена с ролью библиотекаря
        //    var token = _jwtProvider.GenerateToken(librarian);
        //    return token;
        //}

        public async Task<string> Login(string login, string password)
        {
            var librarian = await _librarianRepository.GetByLogin(login);

            if (librarian == null)
            {
                throw new Exception("User not found");
            }

            var result = _passwordHasher.Verify(password, librarian.PasswordHash);

            if (!result)
            {
                throw new Exception("Failed to login");
            }

            // Генерация JWT токена с ролью библиотекаря
            var token = _jwtProvider.GenerateWorkerToken(librarian);
            return (token);
        }


        public async Task<Guid> AddDirector(Librarian librarianReq)
        {
            var hashedPassword = _passwordHasher.Generate(librarianReq.PasswordHash);

            var (librarian, error) = Librarian.Create(
                librarianReq.Id,
                librarianReq.Name,
                librarianReq.Login,
                hashedPassword,
                librarianReq.Role, // Добавлено поле роли
                librarianReq.LibraryId
            );

            if (librarian == null)
            {
                throw new ArgumentException(error); // Или обработка ошибки, если нужно
            }
            return await _librarianRepository.AddDirector(librarian);
        }

        public async Task<Guid> AddLibrarian(Librarian librarianReq)
        {
            var hashedPassword = _passwordHasher.Generate(librarianReq.PasswordHash);

            // Создание нового экземпляра Librarian с обновленными данными
            var (librarian, error) = Librarian.Create(
                librarianReq.Id,
                librarianReq.Name,
                librarianReq.Login,
                hashedPassword, // Используем хешированный пароль
                librarianReq.Role,// Новое поле роли
                librarianReq.ReadingRoomId
            );

            if (librarian == null)
            {
                throw new ArgumentException(error); // Обработка ошибки
            }

            // Обновление библиотекаря в репозитории
            return await _librarianRepository.AddLibrarian(librarian);
        }

        //public async Task<bool> LoginExistsAsync(string login)
        //{
        //    return await _librarianRepository.LoginExistsAsync(login);
        //}

        public async Task<Guid> EditWorkerInfo(Guid id, string? name, string? login, string? passwordHash, Guid? libraryId, Guid? ReadingRoomId)
        {
            return await _librarianRepository.EditInfo(id, name, login, passwordHash, libraryId, ReadingRoomId);
        }
    }
}