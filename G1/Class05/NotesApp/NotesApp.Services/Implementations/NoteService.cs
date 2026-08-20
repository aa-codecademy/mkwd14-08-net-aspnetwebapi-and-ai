using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Mappers;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementations;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITagRepository _tagRepository;

    public NoteService(
        INoteRepository noteRepository,
        IUserRepository userRepository,
        ITagRepository tagRepository)
    {
        _noteRepository = noteRepository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
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

    public NoteDto GetNoteById(int id)
    {
        Note? noteDb = _noteRepository.GetById(id);

        if (noteDb is null)
        {
            throw new NoteNotFoundException($"Note with Id {id} not found.");
        }

        return noteDb.ToNoteDto();
    }

    public NoteDto AddNote(AddNoteDto addNoteDto)
    {
        // 1) Validate
        ValidateText(addNoteDto.Text);

        User user = _userRepository.GetById(addNoteDto.UserId);
        if (user is null)
        {
            throw new UserNotFoundException($"User with id {user.Id} does not exist."); 
        }

        List<Tag> tags = _tagRepository.GetByIds(addNoteDto.TagIds);

        // 2) Map
        Note newNote = addNoteDto.ToNote();
        newNote.Tags = tags;
        newNote.User = user;

        // 3) Save
        _noteRepository.Add(newNote);

        return newNote.ToNoteDto();
    }

    #region Private helpers

    private void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new NoteDataException("Text is a required field.");
        }

        if (text.Length > 100)
        {
            throw new NoteDataException("Text cannot contain more than 100 characters.");
        }
    }

    #endregion

}
