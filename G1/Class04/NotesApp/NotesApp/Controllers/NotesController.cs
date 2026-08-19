using Microsoft.AspNetCore.Mvc;
using NotesApp.Domain.Enums;
using NotesApp.Dtos;
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
    // GET: /api/notes?priority=High
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
            throw;
        }
    }

}
