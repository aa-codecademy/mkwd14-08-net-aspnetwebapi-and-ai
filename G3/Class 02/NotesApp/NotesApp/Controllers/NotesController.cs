using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")] //http:localhost:[port]/api/notes
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> GetNotes()
        {
            //return StatusCode(StatusCodes.Status200OK, StaticDb.SimpleNotes); //status code 200
            return Ok(StaticDb.SimpleNotes); //status code 200
        }

        [HttpGet("{index}")] //http:localhost:[port]/api/notes/1
        public ActionResult<string> GetByIndex(int index)
        {
            try
            {
                //validation
                if (index < 0)
                {
                    return BadRequest("The index cannot have a negative value");
                }

                if (index >= StaticDb.SimpleNotes.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Note with index {index} does not exist");
                }

                return Ok(StaticDb.SimpleNotes[index]);
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        [HttpGet("{noteId}/{userId}")] //http:localhost:[port]/api/notes/1/2
        public ActionResult<string> GetNoteByNoteAndUserId(int noteId, int userId)
        {
            try
            {
                if (noteId < 0 || userId < 0)
                {

                    return BadRequest("The ids cannot have negative values");
                }

                return Ok($"Returning note with id {noteId} and user: {userId}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        [HttpPost] //http:localhost:[port]/api/notes
        public ActionResult Post([FromBody] string newNote)
        {
            try
            {
                StaticDb.SimpleNotes.Add(newNote);
                return StatusCode(StatusCodes.Status201Created, "The new note was added");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }
    }
}
