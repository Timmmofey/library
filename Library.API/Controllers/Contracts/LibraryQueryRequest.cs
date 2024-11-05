namespace Library.API.Controllers.Contracts
{
    public class LibraryQueryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
