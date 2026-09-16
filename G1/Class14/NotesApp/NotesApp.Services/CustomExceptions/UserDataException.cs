namespace NotesApp.Services.CustomExceptions;

public class UserDataException : Exception
{
    public UserDataException(string message) : base(message)
    {
        
    }
}
