namespace Library.DataAccess.Entities
{
    public class ReaderCategoryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ReaderEntity> Readers { get; set; }
    }
}
