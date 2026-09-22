using NotesApp.Domain.Models;
using NotesApp.DTOs;

namespace NotesApp.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto ToNoteDto(this Note note)
        {
            return new NoteDto
            {
                Id = note.Id,
                Text = note.Text,
                UserFullName = note.User != null ? $"{note.User.FirstName} {note.User.LastName}" : "Unknown",
                Priority = note.Priority,
                Tags = note.Tags.Select(x => x.ToTagDto()).ToList()
            };
        }

        public static Note ToNote(this AddNoteDto addNote)
        {
            return new Note
            {
                Text = addNote.Text,
                Priority = addNote.Priority,
                UserId = addNote.UserId
            };
        }
    }
}
