using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations.EntityFramework;

public class NoteRepository : INoteRepository
{
    private readonly NotesAppDbContext _context;

    public NoteRepository(NotesAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetAllAsync()
    {
        var notes = _context.Notes
            .Include(note => note.Tags)
            .Include(note => note.User)
            .AsQueryable();

        // ToListAsync, FirstOrDefault, FirstOrDefaultAsync, SingleOrDefault, SingleOrDefaultAsync, ToList, ToArray, ToDictionary, ToLookup, Count, LongCount, Any, All, Contains, First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault
        // You can use any of the above methods to execute the query and retrieve the results.

        return await notes.ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _context.Notes
            .Include(note => note.Tags)
            .Include(note => note.User)
            .FirstOrDefaultAsync(note => note.Id == id);
    }

    public async Task<List<Note>> GetByIdsAsync(List<int> ids)
    {
        return await _context.Notes
            .Include(note => note.Tags)
            .Include(note => note.User)
            .Where(note => ids.Contains(note.Id))
            .ToListAsync();
    }

    public async Task AddAsync(Note entity)
    {
        _context.Notes.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note entity)
    {
        //_context.Notes.Update(entity); // this is needed if the entity is not tracked by the context
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Note entity)
    {
        _context.Notes.Remove(entity);
        await _context.SaveChangesAsync();
    }
   
}
