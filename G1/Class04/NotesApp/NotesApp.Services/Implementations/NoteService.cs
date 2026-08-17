using NotesApp.DataAccess.Interfaces;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementations;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }
}
