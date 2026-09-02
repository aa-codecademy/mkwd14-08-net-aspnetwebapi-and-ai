using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Enums;
using LibraryApi.Domain.Models;
using LibraryApi.Dtos;
using LibraryApi.Mappers;
using LibraryApi.Services.CustomExceptions;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<List<BookDto>> GetAllBooksAsync(Genre? genre = null, int? minYear = null)
    {
        // 1) Get all books from the database
        List<Book> booksDb = await _bookRepository.GetAllAsync();

        // 2) Optional filters
        if (genre.HasValue)
        {
            booksDb = booksDb.Where(book => book.Genre == genre.Value).ToList();
        }

        if (minYear.HasValue)
        {
            booksDb = booksDb.Where(book => book.Year > minYear.Value).ToList();
        }

        // 3) Map to DTOs
        return booksDb.ToBookDtoList();
    }

    public async Task<BookDto> GetBookByIdAsync(int id)
    {
        Book? bookDb = await _bookRepository.GetByIdAsync(id);

        if (bookDb is null)
        {
            throw new BookNotFoundException($"Book with id {id} was not found.");
        }

        return bookDb.ToBookDto();
    }

    public async Task<List<BookDto>> GetBooksByAuthorAsync(int authorId)
    {
        Author? author = await _authorRepository.GetByIdAsync(authorId);

        if (author is null)
        {
            throw new AuthorNotFoundException($"Author with id {authorId} was not found.");
        }

        List<Book> booksDb = await _bookRepository.GetByAuthorIdAsync(authorId);

        return booksDb.ToBookDtoList();
    }

    public async Task<BookDto> AddBookAsync(AddBookDto addBookDto)
    {
        // 1) Validate
        ValidateTitle(addBookDto.Title);
        ValidateIsbn(addBookDto.Isbn);
        ValidateYear(addBookDto.Year);
        ValidatePageCount(addBookDto.PageCount);
        ValidateGenre(addBookDto.Genre);

        Author? author = await _authorRepository.GetByIdAsync(addBookDto.AuthorId);
        if (author is null)
        {
            throw new AuthorNotFoundException($"Author with id {addBookDto.AuthorId} does not exist.");
        }

        // 2) Map
        Book newBook = addBookDto.ToBook();
        newBook.Author = author;

        // 3) Save
        _bookRepository.AddAsync(newBook);

        return newBook.ToBookDto();
    }

    public async Task UpdateBookAsync(UpdateBookDto updateBookDto)
    {
        // 1) Validate
        Book? bookDb = await _bookRepository.GetByIdAsync(updateBookDto.Id);

        if (bookDb is null)
        {
            throw new BookNotFoundException($"Book with id {updateBookDto.Id} was not found.");
        }

        ValidateTitle(updateBookDto.Title);
        ValidateIsbn(updateBookDto.Isbn);
        ValidateYear(updateBookDto.Year);
        ValidatePageCount(updateBookDto.PageCount);
        ValidateGenre(updateBookDto.Genre);

        // 2) Map
        updateBookDto.ApplyTo(bookDb);

        // 3) Save
        await _bookRepository.UpdateAsync(bookDb);
    }

    public async Task DeleteBookAsync(int id)
    {
        Book? bookDb = await _bookRepository.GetByIdAsync(id);

        if (bookDb is null)
        {
            throw new BookNotFoundException($"Book with id {id} was not found.");
        }

        await _bookRepository.DeleteAsync(bookDb);
    }

    #region Private helpers

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BookDataException("Title is a required field.");
        }

        if (title.Length > 200)
        {
            throw new BookDataException("Title cannot contain more than 200 characters.");
        }
    }

    private static void ValidateIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new BookDataException("Isbn is a required field.");
        }

        if (isbn.Length > 20)
        {
            throw new BookDataException("Isbn cannot contain more than 20 characters.");
        }
    }

    private static void ValidateYear(int year)
    {
        if (year < 1450 || year > DateTime.UtcNow.Year)
        {
            throw new BookDataException($"Year '{year}' is not a valid publication year.");
        }
    }

    private static void ValidatePageCount(int pageCount)
    {
        if (pageCount <= 0)
        {
            throw new BookDataException("PageCount must be greater than zero.");
        }
    }

    private static void ValidateGenre(Genre genre)
    {
        // An enum is just a number underneath, so "genre": 42 binds happily. We have to check it ourselves.
        if (!Enum.IsDefined(genre))
        {
            throw new BookDataException($"Genre '{genre}' is not a valid value.");
        }
    }

    #endregion
}
