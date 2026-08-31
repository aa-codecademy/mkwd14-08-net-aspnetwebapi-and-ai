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

    public async Task<List<NoteDto>> GetAllNotesAsync(Priority? priority = null)
    {
        // 1) Get all notes from db
        var notesDbTask = _noteRepository.GetAllAsync();
        List<Note> notesDb = await notesDbTask;

        // Optional filter
        if (priority.HasValue)
        {
            notesDb = notesDb.Where(note => note.Priority == priority).ToList();
        }

        // 2) Map notes from db to dto
        List<NoteDto> noteDtos = notesDb.ToNoteDtoList();

        return noteDtos;
    }

    public async Task<NoteDto> GetNoteByIdAsync(int id)
    {
        Note? noteDb = await _noteRepository.GetByIdAsync(id);

        if (noteDb is null)
        {
            throw new NoteNotFoundException($"Note with Id {id} not found.");
        }

        return noteDb.ToNoteDto();
    }

    public async Task<NoteDto> AddNoteAsync(AddNoteDto addNoteDto)
    {
        // 1) Validate
        ValidateText(addNoteDto.Text);
        ValidatePriority(addNoteDto.Priority);

        User? user = await _userRepository.GetByIdAsync(addNoteDto.UserId);
        if (user is null)
        {
            throw new UserNotFoundException($"User with id {addNoteDto.UserId} does not exist."); 
        }

        List<Tag> tags = await _tagRepository.GetByIdsAsync(addNoteDto.TagIds);

        // 2) Map
        Note newNote = addNoteDto.ToNote();
        newNote.Tags = tags;
        newNote.User = user;

        // 3) Save
        await _noteRepository.AddAsync(newNote);

        return newNote.ToNoteDto();
    }

    public async Task UpdateNoteAsync(UpdateNoteDto updateNoteDto)
    {
        // 1) Validate
        Note? noteDb = await _noteRepository.GetByIdAsync(updateNoteDto.Id);

        if (noteDb is null)
        {
            throw new NoteNotFoundException($"Note with id {updateNoteDto.Id} was not found.");
        }

        ValidateText(updateNoteDto.Text);
        ValidatePriority(updateNoteDto.Priority);

        List<Tag> tags = await _tagRepository.GetByIdsAsync(updateNoteDto.TagIds);

        // 2) Map
        updateNoteDto.ApplyTo(noteDb);
        noteDb.Tags = tags;

        // 3) Save
        await _noteRepository.UpdateAsync(noteDb);
    }

    public async Task DeleteNoteAsync(int id)
    {
        Note? noteDb = await _noteRepository.GetByIdAsync(id);

        if (noteDb is null)
        {
            throw new NoteNotFoundException($"Note with id {id} was not found.");
        }

        await _noteRepository.DeleteAsync(noteDb);
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

    private static void ValidatePriority(Priority priority)
    {
        // An enum is just a number underneath, so "priority": 42 binds happily.
        // We have to check it ourselves.
        if (!Enum.IsDefined(priority))
        {
            throw new NoteDataException($"Priority '{priority}' is not a valid value.");
        }
    }

    #endregion

}
