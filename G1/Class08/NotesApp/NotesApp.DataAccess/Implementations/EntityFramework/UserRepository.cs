using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations.EntityFramework;

public class UserRepository : IUserRepository
{
    private readonly NotesAppDbContext _context;

    public UserRepository(NotesAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetByIdsAsync(List<int> ids)
    {
        return await _context.Users
            .Where(u => ids.Contains(u.Id))
            .ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

}
