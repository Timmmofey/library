namespace Library.DataAccess.Entities
{
    public class ShelfEntity
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Number { get; set; }

        public SectionEntity Section { get; set; }
        public ICollection<ItemCopyEntity> ItemCopies { get; set; }
    }
}
