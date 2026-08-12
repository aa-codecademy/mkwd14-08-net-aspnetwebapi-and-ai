using Microsoft.AspNetCore.Mvc;

namespace Class02.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        // GET: https://localhost:[port]/api/notes
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(StaticDb.SimpleNotes);
        }

        // GET: https://localhost:[port]/api/notes/1

        /// <summary>
        /// Gets a not by its id.
        /// </summary>
        /// <param name="id">The id of the note entity to be returned</param>
        /// <response code="200">Returns the note by its id.</response>
        /// <response code="404">If the note is not found or invalid id is provided.</response>
        [HttpGet("{id:int}")]
        public ActionResult<string> GetById(int id)
        {
            if (id < 0 || id >= StaticDb.SimpleNotes.Count)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Note with id {id} does not exist."
                });
            }
            return Ok(StaticDb.SimpleNotes[id]);
        }

        // GET: https://localhost:[port]/api/notes/5/user/2
        [HttpGet("{noteId:int}/user/{userId:int}")]
        public ActionResult<string> GetNotForUser(int noteId, int userId)
        {
            if (noteId < 0 || userId < 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid note or user ID."
                });
            }
            return Ok($"Note ID: {noteId} for User ID: {userId}");
        }

        // POST: https://localhost:[port]/api/notes
        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                using (StreamReader sr = new StreamReader(Request.Body))
                {
                    string newNote = sr.ReadToEnd();
                    if(string.IsNullOrEmpty(newNote))
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            Message = "Note title cannot be empty."
                        });
                    }
                    StaticDb.SimpleNotes.Add(newNote);
                    return StatusCode(StatusCodes.Status201Created, "The new note was successfully created!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing the request.",
                    Error = ex.Message
                });
            }
        }
    }
}
