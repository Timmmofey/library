using Library.API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Controllers.Contracts
{
    public record LoginLibrarianRequest
    (
        [Required] string Login,
        [Required] string Password
    );
}

//public async Task<(string token, string[] roles)> Login(string login, string password)
//{
//    // Логика проверки учетных данных и получения пользователя
//    var librarian = await GetLibrarianByLoginAsync(login);
//    if (librarian == null || !VerifyPassword(password, librarian.PasswordHash))
//    {
//        throw new UnauthorizedAccessException("Неверный логин или пароль.");
//    }

//    // Генерация токена
//    var token = GenerateToken(librarian);

//    // Возврат токена и ролей
//    return (token, librarian.Roles); // Предполагается, что у вас есть поле Roles в модели Librarian
//}

//[HttpPost("login")]
//public async Task<ActionResult<LoginResponse>> LoginLibrarian(LoginLibrarianRequest request)
//{
//    var (token, roles) = await _librarianService.Login(request.Login, request.Password);

//    var response = new LoginResponse
//    {
//        Token = token,
//        Roles = roles
//    };

//    return Ok(response);
//}

//namespace Library.API.Controllers.Contracts
//{
//    public record LoginResponse
//    (
//        string Token,
//        string[] Roles
//    );
//}