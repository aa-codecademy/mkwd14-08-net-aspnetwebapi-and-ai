using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.DataAccess.Implementations.EntityFramework;

// 1) Tracking entities => EF Core keeps track of changes to entities, so you can call SaveChangesAsync() without explicitly updating the entity. EF Core will detect changes and generate the appropriate SQL commands to update the database.
// This feature has a cost in terms of performance and memory usage, especially when dealing with large datasets. If you don't need tracking, you can use AsNoTracking() to improve performance.

// 2) IQueryable => EF Core uses IQueryable to build SQL queries dynamically based on LINQ expressions. This allows for efficient querying and filtering of data directly in the database, rather than loading all data into memory and filtering it in the application.

// 3) Projections => EF Core supports projections, allowing you to select only the necessary fields from the database, reducing the amount of data transferred and improving performance.

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
            .AsNoTracking()  // Use AsNoTracking for read-only queries to improve performance
            .Include(note => note.Tags)
            .Include(note => note.User)
            .AsQueryable();

        // ToListAsync, FirstOrDefault, FirstOrDefaultAsync, SingleOrDefault, SingleOrDefaultAsync, ToList, ToArray, ToDictionary, ToLookup, Count, CountAsync, LongCount, Any, All, Contains, First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault
        // You can use any of the above methods to execute the query and retrieve the results.

        return await notes.ToListAsync();
    }

    public async Task<List<NoteDto>> GetAllByPriorityAsync(Priority? priority = null)
    {
        // 1) Build the query
        // IQueryable is a recipe for a query, not the query itself. The database is not touched until step 4.
        IQueryable<Note> query = _context.Notes;

        // 2) Filter (if needed)
        if (priority.HasValue)
        {
            query = query.Where(note => note.Priority == priority);
        }

        // 3) Project the query to NoteDto
        // Projections with .Select() allow you to select only the necessary fields from the database, reducing the amount of data transferred and improving performance.
        // This is especially useful when you have complex entities with many properties, and you only need a subset of those properties for your application logic.
        // The projection is done in the database, not in memory !!!. We avoid in memory transformations, which can be costly in terms of performance and memory usage.
        var noteDtoQuery = query.Select(note => new NoteDto
        {
            Id = note.Id,
            Priority = note.Priority,
            Text = note.Text,
            // Flattened in SQL, not in C#: a ternary becomes CASE WHEN.
            UserFullName = note.User == null ? "Unknown" : $"{note.User.FirstName} {note.User.LastName}",

            // A nested projection. EF turns this into a second JOIN and stitches
            // the tags onto each note itself.
            Tags = note.Tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Color = tag.Color,
            }).ToList(),
            CreatedDate = note.CreatedDate,
            UpdatedDate = note.UpdatedDate,
        });

        // Note: Here the query is still being built, not executed. The database is not touched until step 4.

        // We can check the generated SQL query for debugging purposes (using ToQueryString()). This is useful for understanding how LINQ expressions are translated into SQL.
        string queryString = query.ToQueryString();

        // 4) Execute the query and return the results (Materialization of the query)
        // This is the only step that actually talks to the database. The query is executed, and the results are materialized into a list of NoteDto objects.
        return await noteDtoQuery.ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _context.Notes
            //.AsNoTracking()
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
