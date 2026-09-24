using LibraryApi.DataAccess.Data;
using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .Include(book => book.Author)
                .AsNoTracking()
                .ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(book => book.Author)
                .AsNoTracking()
                .FirstOrDefault(book => book.Id == id);
        }

        public List<Book> GetByAuthorId(int authorId)
        {
            return _context.Books
                .Where(book => book.AuthorId == authorId)
                .AsNoTracking()
                .ToList();
        }

        public void Add(Book entity)
        {
            _context.Books.Add(entity);
        }

        public void Update(Book entity)
        {
            _context.SaveChanges();
        }

        public void Delete(Book entity)
        {
            _context.Books.Remove(entity);
            _context.SaveChanges();
        }
    }
}
