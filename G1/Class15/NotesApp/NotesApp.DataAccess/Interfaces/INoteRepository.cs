using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.DataAccess.Interfaces;

public interface INoteRepository : IRepository<Note>
{
    Task<List<NoteDto>> GetAllByPriorityAsync(int userId, Priority? priority = null);
    Task<List<Note>> GetAllAsync(int userId); // Method overloading 
}
