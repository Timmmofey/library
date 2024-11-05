using System.ComponentModel.DataAnnotations;

namespace Library.API.Controllers.Contracts
{
    public record RegisterLibrarianRequest
    (
        [Required] string Name,
        [Required] string Login,
        [Required] string Password
    );
}
