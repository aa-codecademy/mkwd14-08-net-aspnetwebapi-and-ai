using LibraryApi.Domain.Enums;
using LibraryApi.Dtos;
using LibraryApi.Services.CustomExceptions;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<BookDto>> GetAll([FromQuery] Genre? genre = null, [FromQuery] int? minYear = null)
        {
            try
            {
                var books = _bookService.GetAllBooks(genre, minYear);
                return Ok(books);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<BookDto> GetById(int id)
        {
            try
            {
                BookDto book = _bookService.GetBookById(id);
                return Ok(book);
            }
            catch (BookNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }

        [HttpGet("by-author/{authorId:int}")]
        public ActionResult<List<BookDto>> GetByAuthor(int authorId)
        {
            try
            {
                var books = _bookService.GetBooksByAuthor(authorId);
                return Ok(books);
            }
            catch (AuthorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddBookDto addBookDto)
        {
            try
            {
                BookDto newBook = _bookService.AddBook(addBookDto);

                return CreatedAtAction(nameof(GetById),
                    new { id = newBook.Id },
                    newBook);
            }
            catch (AuthorNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BookDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateBookDto updateBookDto)
        {
            try
            {
                _bookService.UpdateBook(updateBookDto);
                return NoContent();
            }
            catch (BookNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BookDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _bookService.DeleteBook(id);
                return NoContent();
            }
            catch (BookNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occured. Please contact your administrator");
            }
        }
    }
}
