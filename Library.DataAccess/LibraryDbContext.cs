using Microsoft.EntityFrameworkCore;
using Library.DataAccess.Entities;
using Library.DataAccess.Configurations;

namespace Library.DataAccess
{
    public class LibraryDbContext: DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
        { 
            
        }

        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<ItemCategoryEntity> ItemCategories { get; set; }
        public DbSet<ItemCopyEntity> ItemCopies { get; set; }
        public DbSet<ItemEntity> Items {  get; set; }

        public DbSet<LibrarianEntity> Librarians { get; set; }
        public DbSet<LibraryEntity> Libraries { get; set; }
        public DbSet<LoanEntity> Loans { get; set; }
        public DbSet<ReadingRoomEntity> ReadingRooms { get; set; }


        public DbSet<ReaderEntity> Readers { get; set; }
        public DbSet<ReaderCategoryEntity> ReaderCategories { get; set; }
        public DbSet<SectionEntity> Sections { get; set; }
        public DbSet<ShelfEntity> Shelfs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCopyConfiguration());
            modelBuilder.ApplyConfiguration(new LibrarianConfiguration());
            modelBuilder.ApplyConfiguration(new LibraryConfiguration());
            modelBuilder.ApplyConfiguration(new LoanConfiguration());
            modelBuilder.ApplyConfiguration(new ReaderCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ReaderConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new ShelfConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingRoomConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}
