using LibraryApi.Domain.Enums;
using LibraryApi.Dtos;

namespace LibraryApi.Services.Interfaces
{
    public interface IBookService
    {
        List<BookDto> GetAllBooks(Genre? genre = null, int? minYear = null);
        BookDto GetBookById(int id);
        List<BookDto> GetBooksByAuthor(int authorId);
        BookDto AddBook(AddBookDto addBookDto);
        void UpdateBook(UpdateBookDto updateBookDto);
        void DeleteBook(int id);
    }
}
