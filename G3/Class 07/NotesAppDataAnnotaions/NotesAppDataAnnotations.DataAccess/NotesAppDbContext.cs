using Microsoft.EntityFrameworkCore;
using NotesAppDataAnnotations.Domain;

namespace NotesAppDataAnnotations.DataAccess
{
    public class NotesAppDbContext : DbContext
    {
        public NotesAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
