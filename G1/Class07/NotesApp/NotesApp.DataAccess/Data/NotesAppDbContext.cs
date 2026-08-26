using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Helpers;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Data
{
    /// <summary>
    /// Our database, as far as the C# code is concerned. 
    /// It does two things: 
    /// 1) every DbSet below becomes a table, and 
    /// 2) OnModelCreating() says what those tables look like.
    /// </summary>
    public class NotesAppDbContext : DbContext
    {
        // EF Core passes in the options (provider + connection string).
        // Program.cs decides what they are; we never build them here.
        public NotesAppDbContext(DbContextOptions<NotesAppDbContext> options) : base(options)
        {
        }

        // The DbSet properties are the tables in our database. Each one is a collection of entities of that type.
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // This method is called by EF Core when it is building the model.
        // We can use it to configure the model, e.g. to set up relationships, constraints, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===> Configure entities 
            modelBuilder.ConfigureNote();
            //modelBuilder.ConfigureTag();
            //modelBuilder.ConfigureUser();

            // ===> Seed data
            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }

    }
}
