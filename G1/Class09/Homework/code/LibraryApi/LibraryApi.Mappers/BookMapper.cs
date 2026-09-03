using LibraryApi.Domain.Models;
using LibraryApi.Dtos;

namespace LibraryApi.Mappers;

public static class BookMapper
{
    public static BookDto ToBookDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            Year = book.Year,
            Genre = book.Genre,
            AuthorFullName = book.Author is null
                ? "Unknown"
                : book.Author.FullName
        };
    }

    public static List<BookDto> ToBookDtoList(this List<Book> books) => books.Select(book => book.ToBookDto()).ToList();

    public static Book ToBook(this AddBookDto addBookDto)
    {
        return new Book
        {
            Title = addBookDto.Title,
            Isbn = addBookDto.Isbn,
            Year = addBookDto.Year,
            PageCount = addBookDto.PageCount,
            Genre = addBookDto.Genre,
            AuthorId = addBookDto.AuthorId
        };
    }

    public static void ApplyTo(this UpdateBookDto updateBookDto, Book existingBook)
    {
        existingBook.Title = updateBookDto.Title;
        existingBook.Isbn = updateBookDto.Isbn;
        existingBook.Year = updateBookDto.Year;
        existingBook.PageCount = updateBookDto.PageCount;
        existingBook.Genre = updateBookDto.Genre;
        existingBook.UpdatedDate = DateTime.UtcNow;
    }
}
