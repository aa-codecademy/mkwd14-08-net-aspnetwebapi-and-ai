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
    public async Task<ActionResult<List<NoteDto>>> GetAll([FromQuery] Priority? priority = null)
    {
        try
        {
            List<NoteDto> result = await _noteService.GetAllNotesAsync(priority);
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
    public async Task<ActionResult<NoteDto>> GetById(int id)
    {
        try
        {
            NoteDto noteDto = await _noteService.GetNoteByIdAsync(id);
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
    public async Task<ActionResult<NoteDto>> Create([FromBody] AddNoteDto noteDto)
    {
        try
        {
            NoteDto createdDto = await _noteService.AddNoteAsync(noteDto);

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

    // PUT /api/notes/
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        try
        {
            await _noteService.UpdateNoteAsync(updateNoteDto);

            // 204: it worked, and there is nothing worth sending back.
            return NoContent();
        }
        catch (NoteNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (NoteDataException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(
                value: "An error occurred, please contact the administrator.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // DELETE /api/notes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
        catch (NoteNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(
                value: "An error occurred, please contact the administrator.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

}
