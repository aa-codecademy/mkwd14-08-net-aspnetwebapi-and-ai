using Microsoft.EntityFrameworkCore;
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
            //modelBuilder.Entity<Note>()
            //    .Property(note => note.Text)
            //    .IsRequired();

            //modelBuilder.Entity<Note>()
            //  .Property(note => note.Priority)
            //  .IsRequired();

            // Better way to configure the Note entity using the Fluent API
            // FLUENT API - everything about the Note table, in one place.
            // User and Tag are configured on the classes themselves with Data Annotations (Attributes). Two styles, same result.
            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note");

                // Id becomes the key by convention, because BaseEntity calls it "Id".
                // A differently named property would need entity.HasKey(...).

                entity.Property(note => note.Text)
                      .IsRequired()
                      .HasMaxLength(100);

                // An enum is an int underneath, so Priority would be stored as
                // 1 / 2 / 3. HasConversion<string>() stores "High" instead, which
                // is far easier to read in SSMS.
                entity.Property(note => note.Priority)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(30);

                // ===> One to Many relation (1:M)
                // One user has many notes.
                // Cascade: deleting a user deletes their notes, because a note without an owner means nothing.
                entity.HasOne(note => note.User)
                      .WithMany(user => user.Notes)
                      .HasForeignKey(note => note.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // We filter notes by user often, so index the foreign key.
                // Index => a database structure that makes searching faster. It is like the index in a book.
                entity.HasIndex(note => note.UserId);

                // ===> Many to Many relation (M:M)
                // Many-to-many needs a third table.
                // WithMany() is empty because Tag has no Notes list - we only walk this from the note side.
                // The short form .UsingEntity(j => j.ToTable("NoteTag")) also works, but names the column after the navigation property: "TagsId".
                // Spelling the keys out gives us NoteId / TagId.
                entity.HasMany(note => note.Tags)
                      .WithMany()
                      .UsingEntity(
                         "NoteTag",
                         right => right.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagId"),
                         left => left.HasOne(typeof(Note)).WithMany().HasForeignKey("NoteId")
                      );
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
