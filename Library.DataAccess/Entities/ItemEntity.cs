namespace Library.DataAccess.Entities
{
    public class ItemEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<AuthorEntity> Authors { get; set; }
        
        public Guid CategoryId { get; set; }
        public string Publications { get; set; }
        public ItemCategoryEntity Category { get; set; }
        public ICollection<ItemCopyEntity> ItemCopies { get; set; }
    }
}
