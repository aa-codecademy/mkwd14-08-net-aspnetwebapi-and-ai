namespace NotesApp.Services.CustomExceptions;

public class NoteAccessDeniedException : Exception
{
    public NoteAccessDeniedException(string message) : base(message)
    {
    }
}
