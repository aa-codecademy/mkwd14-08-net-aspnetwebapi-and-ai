using LibraryApi.DataAccess.Helpers;
using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Data;

/// <summary>
/// Our database as far as the C# code is concerned.
/// Every DbSet becomes a table; OnModelCreating says what those tables look like.
/// </summary>
public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
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
