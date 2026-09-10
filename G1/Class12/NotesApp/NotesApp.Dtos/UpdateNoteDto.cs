using NotesApp.Domain.Enums;

namespace NotesApp.Dtos;

public class UpdateNoteDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public List<int> TagIds { get; set; } = new List<int>();
}
