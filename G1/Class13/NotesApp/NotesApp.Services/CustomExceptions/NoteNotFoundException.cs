namespace NotesApp.Services.CustomExceptions;

public class NoteNotFoundException : Exception
{
    public string NoteMessage { get; set; }
    public string DefaultMessage { get; } = "Note not found.";
    public NoteNotFoundException(string message)
    {
        NoteMessage = message ?? DefaultMessage;
    }
}
