namespace NotesAppClientApi.Models;

// Only the fields we pass on. The Notes API sends more; the extra JSON
// properties are simply ignored when deserializing.
public class NoteDto
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public Priority Priority { get; set; }
}
