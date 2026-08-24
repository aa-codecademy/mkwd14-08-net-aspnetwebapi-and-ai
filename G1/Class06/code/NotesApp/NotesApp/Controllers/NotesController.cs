using Microsoft.AspNetCore.Mvc;
using NotesApp.Domain.Enums;
using NotesApp.Dtos;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // GET: /api/notes
    // GET: /api/notes?priority=1 (1,2 or 3)
    // NOTE: priority is optional
    [HttpGet]
    public ActionResult<List<NoteDto>> GetAll([FromQuery] Priority? priority = null)
    {
        try
        {
            List<NoteDto> result = _noteService.GetAllNotes(priority);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Logging...
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
        }
    }

    // GET /api/notes/1
    [HttpGet("{id:int}")]
    public ActionResult<NoteDto> GetById(int id)
    {
        try
        {
            NoteDto noteDto = _noteService.GetNoteById(id);
            return Ok(noteDto);
        }
        catch (NoteNotFoundException ex)
        {
            return NotFound(ex.NoteMessage);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
        }
    }

    // POST /api/notes
    [HttpPost]
    public ActionResult<NoteDto> Create([FromBody] AddNoteDto noteDto)
    {
        try
        {
            NoteDto createdDto = _noteService.AddNote(noteDto);

            return Ok(createdDto);
            //return CreatedAtAction(nameof(GetById), new { id = noteDto.Id }, noteDto);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NoteDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
        }

    }


}
