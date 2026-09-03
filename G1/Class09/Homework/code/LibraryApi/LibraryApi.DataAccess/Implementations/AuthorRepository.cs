using LibraryApi.DataAccess.Data;
using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.DataAccess.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(author => author.Id == id);
    }

    public async Task AddAsync(Author entity)
    {
        _context.Authors.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author entity)
    {
        _context.Authors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Author entity)
    {
        _context.Authors.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
