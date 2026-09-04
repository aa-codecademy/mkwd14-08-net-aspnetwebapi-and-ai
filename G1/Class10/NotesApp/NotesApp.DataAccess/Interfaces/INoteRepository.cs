using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.DataAccess.Interfaces;

public interface INoteRepository : IRepository<Note>
{
    Task<List<NoteDto>> GetAllByPriorityAsync(Priority? priority = null);
}
