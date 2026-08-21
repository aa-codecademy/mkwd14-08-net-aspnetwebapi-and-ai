using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DTOs;
using NotesApp.Services.Interfaces;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet] //http:localhost:[port]/api/notes
        public ActionResult<List<NoteDto>> GetAll() //the controller must return dtos instead of domain objects
        {
            try
            {
                var notes = _noteService.GetAllNotes();
                return Ok(notes);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator");
            }
        }

        [HttpGet("{id}")] //http://loclahost:[port]/api/notes/1
        public ActionResult<NoteDto> GetById(int id) { //we return a DTO model

            try
            {
                NoteDto note = _noteService.GetById(id);
                return Ok(note);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator");
            }
        }

        [HttpPost]
        public IActionResult AddNote([FromBody] AddNoteDto note)
        {
            try
            {
               NoteDto newNote = _noteService.AddNote(note);
                return Ok(newNote);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator");
            }
        }

    }
}
