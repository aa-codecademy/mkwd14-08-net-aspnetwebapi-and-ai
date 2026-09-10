namespace NotesApp.Services.CustomExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string? message) : base(message)
        {
        }
    }
}
