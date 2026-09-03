using LibraryApi.DataAccess.Data;
using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Implementations;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(book => book.Author)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(book => book.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<List<Book>> GetByAuthorIdAsync(int authorId)
    {
        return await _context.Books
            .Where(book => book.AuthorId == authorId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Book entity)
    {
        _context.Books.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book entity)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book entity)
    {
        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
