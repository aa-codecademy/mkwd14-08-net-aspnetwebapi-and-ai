using LibraryApi.Domain.Enums;
using LibraryApi.Dtos;

namespace LibraryApi.Services.Interfaces;

public interface IBookService
{
    Task<List<BookDto>> GetAllBooksAsync(Genre? genre = null, int? minYear = null);
    Task<BookDto> GetBookByIdAsync(int id);
    Task<List<BookDto>> GetBooksByAuthorAsync(int authorId);
    Task<BookDto> AddBookAsync(AddBookDto addBookDto);
    Task UpdateBookAsync(UpdateBookDto updateBookDto);
    Task DeleteBookAsync(int id);
}
