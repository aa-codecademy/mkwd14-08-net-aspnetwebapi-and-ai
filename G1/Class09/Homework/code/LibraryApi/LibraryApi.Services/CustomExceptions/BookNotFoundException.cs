namespace LibraryApi.Services.CustomExceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(string message) : base(message)
    {
    }
}
