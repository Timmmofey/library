namespace Library.Core.DTOs
{
    public class ShelfDto
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Number { get; set; }

        public ICollection<ItemCopyDto>? ItemCopies { get; set; } = null;
    }
}
