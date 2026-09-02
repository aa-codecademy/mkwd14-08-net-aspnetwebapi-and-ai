namespace LibraryApi.Services.CustomExceptions;

public class BookDataException : Exception
{
    public BookDataException(string message) : base(message)
    {
    }
}
