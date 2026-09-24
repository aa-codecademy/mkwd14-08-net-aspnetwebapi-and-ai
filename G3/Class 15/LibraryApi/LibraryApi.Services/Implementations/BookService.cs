using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Enums;
using LibraryApi.Domain.Models;
using LibraryApi.Dtos;
using LibraryApi.Mappers;
using LibraryApi.Services.CustomExceptions;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public List<BookDto> GetAllBooks(Genre? genre = null, int? minYear = null)
        {
            var booksDb = _bookRepository.GetAll();

            if (genre.HasValue)
            {
                booksDb = booksDb
                    .Where(book => book.Genre == genre.Value)
                    .ToList();
            }

            if (minYear.HasValue)
            {
                booksDb = booksDb
                    .Where(book => book.Year > minYear.Value)
                    .ToList();
            }

            return booksDb.ToBookDtoList();
        }

        public BookDto GetBookById(int id)
        {
            Book? bookDb = _bookRepository.GetById(id);

            if (bookDb is null)
            {
                throw new BookNotFoundException(
                    $"Book with id {id} was not found.");
            }

            return bookDb.ToBookDto();
        }

        public List<BookDto> GetBooksByAuthor(int authorId)
        {
            Author? author = _authorRepository.GetById(authorId);

            if (author is null)
            {
                throw new AuthorNotFoundException(
                    $"Author with id {authorId} was not found.");
            }

            List<Book> booksDb = _bookRepository.GetByAuthorId(authorId);

            return booksDb.ToBookDtoList();
        }

        public BookDto AddBook(AddBookDto addBookDto)
        {
            ValidateTitle(addBookDto.Title);
            ValidateIsbn(addBookDto.Isbn);
            ValidateYear(addBookDto.Year);
            ValidatePageCount(addBookDto.PageCount);
            ValidateGenre(addBookDto.Genre);

            Author? author = _authorRepository.GetById(addBookDto.AuthorId);

            if (author is null)
            {
                throw new AuthorNotFoundException(
                    $"Author with id {addBookDto.AuthorId} does not exist.");
            }

            Book newBook = addBookDto.ToBook();
            newBook.Author = author;

            _bookRepository.Add(newBook);

            return newBook.ToBookDto();
        }

        public void UpdateBook(UpdateBookDto updateBookDto)
        {
            Book? bookDb = _bookRepository.GetById(updateBookDto.Id);

            if (bookDb is null)
            {
                throw new BookNotFoundException(
                    $"Book with id {updateBookDto.Id} was not found.");
            }

            ValidateTitle(updateBookDto.Title);
            ValidateIsbn(updateBookDto.Isbn);
            ValidateYear(updateBookDto.Year);
            ValidatePageCount(updateBookDto.PageCount);
            ValidateGenre(updateBookDto.Genre);

            updateBookDto.ApplyTo(bookDb);

            _bookRepository.Update(bookDb);
        }

        public void DeleteBook(int id)
        {
            Book? bookDb = _bookRepository.GetById(id);

            if (bookDb is null)
            {
                throw new BookNotFoundException(
                    $"Book with id {id} was not found.");
            }

            _bookRepository.Delete(bookDb);
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BookDataException("Title is a required field.");
            }

            if (title.Length > 200)
            {
                throw new BookDataException(
                    "Title cannot contain more than 200 characters.");
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
                throw new BookDataException(
                    "Isbn cannot contain more than 20 characters.");
            }
        }

        private static void ValidateYear(int year)
        {
            if (year < 1450 || year > DateTime.UtcNow.Year)
            {
                throw new BookDataException(
                    $"Year '{year}' is not a valid publication year.");
            }
        }

        private static void ValidatePageCount(int pageCount)
        {
            if (pageCount <= 0)
            {
                throw new BookDataException(
                    "PageCount must be greater than zero.");
            }
        }

        private static void ValidateGenre(Genre genre)
        {
            if (!Enum.IsDefined(genre))
            {
                throw new BookDataException(
                    $"Genre '{genre}' is not a valid value.");
            }
        }
    }
}
