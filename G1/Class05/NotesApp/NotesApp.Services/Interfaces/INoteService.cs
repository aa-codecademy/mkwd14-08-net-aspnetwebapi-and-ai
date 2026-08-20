using NotesApp.Domain.Enums;
using NotesApp.Dtos;

namespace NotesApp.Services.Interfaces;

public interface INoteService
{
    List<NoteDto> GetAllNotes(Priority? priority = null);
    NoteDto GetNoteById(int id);
    NoteDto AddNote(AddNoteDto addNoteDto);
}
