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
                : note.User.FullName,
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

    public static Note ToNote(this AddNoteDto addNoteDto)
    {
        return new Note
        {
            Text = addNoteDto.Text,
            Priority = addNoteDto.Priority,
            UserId = addNoteDto.UserId,
        };
    }

    public static void ApplyTo(this UpdateNoteDto updateNoteDto, Note existingNote)
    {
        existingNote.Text = updateNoteDto.Text;
        existingNote.Priority = updateNoteDto.Priority;
        existingNote.UpdatedDate = DateTime.UtcNow;
    }
}
