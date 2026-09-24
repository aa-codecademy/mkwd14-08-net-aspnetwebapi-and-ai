using LibraryApi.DataAccess.Helpers;
using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureBook();
            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}
