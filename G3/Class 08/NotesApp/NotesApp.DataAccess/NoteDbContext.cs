using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //we want to add our configurations to the existing impl of this method, that's why we leave this call to the base here and just add our own configuration
            base.OnModelCreating(modelBuilder);

            //User - Notes : one-to-many 
            modelBuilder.Entity<Note>() //we start from note
                .HasOne(x => x.User) //note has one user (this is the 1 part of the relationship)
                .WithMany(x => x.Notes) //the user has many notes
                .HasForeignKey(x => x.UserId); //the foreign key with which we connect these two is the propery UserId

            //we only need one configuration - not both
            //modelBuilder.Entity<User>() //we start from user
            //    .HasMany(x => x.Notes) //user has many notes
            //    .WithOne(x => x.User) //each note has one user
            //    .HasForeignKey(x => x.UserId); //the FK that we use to join these two us the propery UserId

            //Note-Tag many to many relationship

            //it uses the primary keys of each class for FK to represent this many to many relationship
            modelBuilder.Entity<Note>() //we start from note
                 .HasMany(x => x.Tags) //note has many tags
                 .WithMany(x => x.Notes) //each tag has many notes
                 .UsingEntity(x => x.ToTable("NoteTags")); //in db we need a middle table for the mtm relationship

            //we can start from whichever class we want, but we only need one way of configuration
            //modelBuilder.Entity<Tag>()
            //.HasMany(x => x.Notes)
            //.WithMany(x => x.Tags)
            //.UsingEntity(x => x.ToTable("NoteTags"));

            modelBuilder.Entity<Note>()
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Note>()
                .Property(x => x.Priority)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(80);

            modelBuilder.Entity<Tag>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(x => x.Color)
                .IsRequired ();

            modelBuilder.Entity<Tag>()
                .Ignore(x => x.NoteCount); //do not map this propery in the db

        }
    }
}
