namespace Library.DataAccess.Entities
{
    public class ItemCategoryEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ItemEntity> Items { get; set; }

    }
}
