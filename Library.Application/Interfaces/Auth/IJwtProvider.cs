using Library.Core.Models;

namespace Library.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateWorkerToken(Librarian librarian);

        public string GenerateReaderToken(Reader reader);
    }
}
