using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Mappers;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementations;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public List<NoteDto> GetAllNotes(Priority? priority = null)
    {
        // 1) Get all notes from db
        List<Note> notesDb = _noteRepository.GetAll();

        // Optional filter
        if (priority.HasValue)
        {
            notesDb = notesDb.Where(note => note.Priority == priority).ToList();
        }

        // 2) Map notes from db to dto

        // ===> Mapping explained
        // Note note = new();
        // => Here we use the static mapper method to map the note to a NoteDto
        // NoteDto noteDto = NoteMapper.ToNoteDto(note);
        // => Here we use the extension method (defined by the 'this' keyword) to map the note to a NoteDto (BETTER WAY)
        // NoteDto noteDto = note.ToNoteDto();

        // ==> Way 1 (not recommended)
        //notesDb.Select(note => new NoteDto
        //{
        //    Id = note.Id,
        //    ...
        //});

        // ==> Way 2 (slightly better)
        //List<NoteDto> mappedNotes = notesDb.Select(note => note.ToNoteDto()).ToList();

        // ==> Way 3 (best way)
        List<NoteDto> noteDtos = notesDb.ToNoteDtoList();

        return noteDtos;
    }
}
