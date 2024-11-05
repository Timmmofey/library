namespace Library.API.Controllers.Contracts
{
    public class ReaderRegisterRequest
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public Guid LibraryId { get; set; }
    }

}
