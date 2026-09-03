namespace LibraryApi.Services.CustomExceptions;

public class AuthorNotFoundException : Exception
{
    public AuthorNotFoundException(string message) : base(message)
    {
    }
}
