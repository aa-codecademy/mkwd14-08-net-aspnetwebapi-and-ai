using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.Mappers;

public static class NoteMapper
{
    public static NoteDto ToNoteDto(this Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Text = note.Text,
            Priority = note.Priority,
            UserFullName = note.User is null
                ? "Unknown"
                : $"{note.User.FirstName} {note.User.LastName}",
            Tags = note.Tags.ToTagDtoList()
        };
    }

    public static List<NoteDto> ToNoteDtoList(this List<Note> notes)
    {
        Func<Note, NoteDto> noteDtoMapper = note => note.ToNoteDto();
        //return notes.Select(note => note.ToNoteDto()).ToList();
        //return notes.Select(ToNoteDto).ToList();
        return notes.Select(noteDtoMapper).ToList();
    }
}
