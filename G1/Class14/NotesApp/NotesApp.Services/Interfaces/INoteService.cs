using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.Services.Interfaces;

public interface INoteService
{
    Task<List<NoteDto>> GetAllNotesAsync(int userId, Priority? priority = null);
    Task<NoteDto> GetNoteByIdAsync(int id, int userId);
    Task<NoteDto> AddNoteAsync(int userId, AddNoteDto addNoteDto);
    Task UpdateNoteAsync(UpdateNoteDto updateNoteDto, int userId);
    Task DeleteNoteAsync(int id, int userId);
}
