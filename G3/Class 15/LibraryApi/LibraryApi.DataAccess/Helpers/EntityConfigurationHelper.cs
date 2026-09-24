using LibraryApi.Domain.Models;
using LibraryApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Helpers
{
    internal static class EntityConfigurationHelper
    {
        public static void ConfigureBook(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(book => book.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(book => book.Isbn)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(book => book.Genre)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(30);

                entity.HasOne(book => book.Author)
                    .WithMany(author => author.Books)
                    .HasForeignKey(book => book.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(book => book.AuthorId);
            });
        }

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            DateTime seeded = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "George", LastName = "Orwell", Country = "United Kingdom", CreatedDate = seeded, UpdatedDate = seeded },
                new Author { Id = 2, FirstName = "Isaac", LastName = "Asimov", Country = "United States", CreatedDate = seeded, UpdatedDate = seeded },
                new Author { Id = 3, FirstName = "Ursula", LastName = "Le Guin", Country = "United States", CreatedDate = seeded, UpdatedDate = seeded },
                new Author { Id = 4, FirstName = "Yuval", LastName = "Harari", Country = "Israel", CreatedDate = seeded, UpdatedDate = seeded }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Isbn = "9780451524935", Year = 1949, PageCount = 328, Genre = Genre.Fiction, AuthorId = 1, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 2, Title = "Animal Farm", Isbn = "9780452284241", Year = 1945, PageCount = 112, Genre = Genre.Fiction, AuthorId = 1, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 3, Title = "Homage to Catalonia", Isbn = "9780156421171", Year = 1938, PageCount = 232, Genre = Genre.History, AuthorId = 1, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 4, Title = "Foundation", Isbn = "9780553293357", Year = 1951, PageCount = 255, Genre = Genre.Science, AuthorId = 2, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 5, Title = "I, Robot", Isbn = "9780553382563", Year = 1950, PageCount = 253, Genre = Genre.Science, AuthorId = 2, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 6, Title = "A Wizard of Earthsea", Isbn = "9780553383041", Year = 1968, PageCount = 183, Genre = Genre.Fantasy, AuthorId = 3, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 7, Title = "The Left Hand of Darkness", Isbn = "9780441478125", Year = 1969, PageCount = 304, Genre = Genre.Fantasy, AuthorId = 3, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 8, Title = "Sapiens", Isbn = "9780062316097", Year = 2011, PageCount = 443, Genre = Genre.History, AuthorId = 4, CreatedDate = seeded, UpdatedDate = seeded },
                new Book { Id = 9, Title = "Homo Deus", Isbn = "9780062464316", Year = 2015, PageCount = 450, Genre = Genre.History, AuthorId = 4, CreatedDate = seeded, UpdatedDate = seeded }
            );
        }
    }
}
