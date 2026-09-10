using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Helpers;

internal static class EntityConfigurationHelper
{
    public static void ConfigureNote(this ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Note>()
        //    .Property(note => note.Text)
        //    .IsRequired();

        //modelBuilder.Entity<Note>()
        //  .Property(note => note.Priority)
        //  .IsRequired();

        // Better way to configure the Note entity using the Fluent API
        // FLUENT API - everything about the Note table, in one place.
        // Every entity is configured this way: the model classes stay plain C#, the database rules all live here.
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
    }

    public static void ConfigureUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Same as [Table("User")] on the class was.
            entity.ToTable("User");

            // IsRequired() => NOT NULL. HasMaxLength(100) => nvarchar(100) instead of nvarchar(max).
            entity.Property(user => user.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(user => user.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(user => user.Username)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.Property(user => user.Password)
                  .IsRequired()
                  .HasMaxLength(100);

            // FullName is computed in C#, so it gets no column. This is the [NotMapped] of the Fluent API.
            // Leave it out and EF Core fails at startup: it cannot map a property that has no setter.
            entity.Ignore(user => user.FullName);

            // Two users cannot share a username - the database enforces it, not only our code.
            entity.HasIndex(user => user.Username)
                  .IsUnique();

            // The other side of the 1:M (User -> Notes) is already configured in ConfigureNote().
            // Configure a relationship once, from one side only.
        });
    }

    public static void ConfigureTag(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.Property(tag => tag.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(tag => tag.Color)
                  .IsRequired()
                  .HasMaxLength(20);
            // .HasColumnName("HexColor") would give the column a different name than the property. We keep them the same.

            entity.HasIndex(tag => tag.Name)
                  .IsUnique();
        });
    }

    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Bob",
                LastName = "Bobsky",
                Password = "SuperSecret123",
                Username = "bob"
            },
            new User
            {
                Id = 2,
                FirstName = "Petko",
                LastName = "Petkovsky",
                Password = "AlsoSecret456",
                Username = "petko"
            }
        );

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Homework", Color = "cyan" },
            new Tag { Id = 2, Name = "Avenga", Color = "orange" },
            new Tag { Id = 3, Name = "Healthy", Color = "green" },
            new Tag { Id = 4, Name = "Exercise", Color = "blue" },
            new Tag { Id = 5, Name = "Urgent", Color = "red" }
        );

        modelBuilder.Entity<Note>().HasData(
            new Note { Id = 1, Text = "Do Homework", Priority = Priority.High, UserId = 1 },
            new Note { Id = 2, Text = "Drink more water", Priority = Priority.Medium, UserId = 1 },
            new Note { Id = 3, Text = "Go to the gym", Priority = Priority.Low, UserId = 2 }
        );

        modelBuilder.Entity("NoteTag").HasData(
            new { NoteId = 1, TagId = 1 },
            new { NoteId = 1, TagId = 2 },
            new { NoteId = 2, TagId = 3 },
            new { NoteId = 3, TagId = 4 },
            new { NoteId = 3, TagId = 5 }
        );
    }
}
