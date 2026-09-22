namespace NotesApp.Services.CustomExceptions;

public class NoteNotFoundException : Exception
{
    public string DefaultMessage { get; } = "Note not found.";
    public NoteNotFoundException(string message) : base(message)
    {
    }
}
