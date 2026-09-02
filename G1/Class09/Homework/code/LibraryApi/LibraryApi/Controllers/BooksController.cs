using LibraryApi.Domain.Enums;
using LibraryApi.Dtos;
using LibraryApi.Services.CustomExceptions;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET /api/books
    // GET /api/books?genre=Fantasy
    // GET /api/books?minYear=1950
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] Genre? genre = null, [FromQuery] int? minYear = null)
    {
        try
        {
            List<BookDto> result = await _bookService.GetAllBooksAsync(genre, minYear);
            return Ok(result);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }

    // GET /api/books/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        try
        {
            BookDto bookDto = await _bookService.GetBookByIdAsync(id);
            return Ok(bookDto);
        }
        catch (BookNotFoundException ex)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Book not found", detail: ex.Message);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }

    // GET /api/books/by-author/1
    [HttpGet("by-author/{authorId:int}")]
    public async Task<ActionResult<List<BookDto>>> GetByAuthor(int authorId)
    {
        try
        {
            List<BookDto> result = await _bookService.GetBooksByAuthorAsync(authorId);
            return Ok(result);
        }
        catch (AuthorNotFoundException ex)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Author not found", detail: ex.Message);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }

    // POST /api/books
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] AddBookDto addBookDto)
    {
        try
        {
            BookDto createdDto = await _bookService.AddBookAsync(addBookDto);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }
        catch (AuthorNotFoundException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid author", detail: ex.Message);
        }
        catch (BookDataException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid book data", detail: ex.Message);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }

    // PUT /api/books
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBookDto updateBookDto)
    {
        try
        {
            await _bookService.UpdateBookAsync(updateBookDto);

            // 204: it worked, and there is nothing worth sending back.
            return NoContent();
        }
        catch (BookNotFoundException ex)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Book not found", detail: ex.Message);
        }
        catch (BookDataException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid book data", detail: ex.Message);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }

    // DELETE /api/books/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
        catch (BookNotFoundException ex)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Book not found", detail: ex.Message);
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error",
                detail: "An error occurred, please contact the administrator.");
        }
    }
}
