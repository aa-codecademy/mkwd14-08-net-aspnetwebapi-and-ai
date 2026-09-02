using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations.EntityFramework;

public class TagRepository : ITagRepository
{
    private readonly NotesAppDbContext _context;

    public TagRepository(NotesAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Tag>> GetByIdsAsync(List<int> ids)
    {
        return await _context.Tags
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();
    }

    public async Task AddAsync(Tag entity)
    {
        _context.Tags.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag entity)
    {
        _context.Tags.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tag entity)
    {
        _context.Tags.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
