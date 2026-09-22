using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotesAppClientApi.Configuration;
using NotesAppClientApi.Models;
using NotesAppClientApi.Services;

namespace NotesAppClientApi.Controllers;

/// <summary>
/// This API has no database. It answers by calling the Notes API through
/// NotesService - which arrives injected, already holding a configured client.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly NotesApiSettings _notesApiSettings;
    private readonly NotesService _notesService;

    public NotesController(
        IOptions<NotesApiSettings> notesApiSettings, 
        NotesService notesService)
    {
        _notesApiSettings = notesApiSettings.Value;
        _notesService = notesService;
    }

    // GET /api/notes
    [HttpGet]
    public async Task<ActionResult<List<NoteDto>>> GetAll()
    {
        try
        {
            await _notesService.LoginAsync(_notesApiSettings.Username, _notesApiSettings.Password);

            List<NoteDto> notes = await _notesService.GetNotesAsync();

            return Ok(notes);
        }
        catch (HttpRequestException ex)
        {
            return Problem(
                title: "The Notes API could not be reached",
                detail: ex.Message,
                statusCode: StatusCodes.Status502BadGateway);
        }
    }
}
