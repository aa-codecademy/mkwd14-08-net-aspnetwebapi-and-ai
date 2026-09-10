using NotesApp.Domain.Enums;
using NotesApp.Dtos;

namespace NotesApp.Services.Interfaces;

public interface INoteService
{
    Task<List<NoteDto>> GetAllNotesAsync(Priority? priority = null);
    Task<NoteDto> GetNoteByIdAsync(int id);
    Task<NoteDto> AddNoteAsync(AddNoteDto addNoteDto);
    Task UpdateNoteAsync(UpdateNoteDto updateNoteDto);
    Task DeleteNoteAsync(int id);
}
