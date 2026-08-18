using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Models.Enums;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/notes
        public ActionResult<List<Note>> Get()
        {
            try
            {
                return Ok(StaticDb.Notes);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administator!");
            }
        }

        [HttpGet("{index}")] //http://localhost:[port]/api/notes/1 - without the index, the route would be the same as the one above. The path variable is a part of this route - it makes this route unique
        public ActionResult<Note> GetByIndex(int index)
        {
            try
            {
                //validation
                if (index < 0)
                {
                    return BadRequest("The index cannot have a negative value");
                }

                if (index >= StaticDb.Notes.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Note with index {index} does not exist");
                }

                return Ok(StaticDb.Notes[index]);
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        [HttpGet("queryString")] //http://localhost:[port]/api/notes/queryString - this is the route

        //http://localhost:[port]/api/notes/queryString?index=1 - we can attach our query string to our route. It is optional. And it does not identi
        public ActionResult<Note> GetByQueryIndex(int? index) //optional
        {
            try
            {
                if (index == null) //beacuse index is an optional param, if it is not sent always return the first note
                {
                    return Ok(StaticDb.Notes[0]);
                }
                //validation
                if (index < 0)
                {
                    return BadRequest("The index cannot have a negative value");
                }

                if (index >= StaticDb.Notes.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Note with index {index} does not exist");
                }

                return Ok(StaticDb.Notes[index.Value]);
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }


        [HttpGet("multipleQuery")]  //http:localhost:[port]/api/notes/multipleQuery  - this is the "main" route. This way both text and priority are null
                                    //http:localhost:[port]/api/notes/multipleQuery?text=gym - we decided to only send the text, the value of priority is null
                                    //http:localhost:[port]/api/notes/multipleQuery?priority=1 - we decided to only send the priority, the value of text is null
                                    //http:localhost:[port]/api/notes/multipleQuery?text=gym&priority=1 - here we sent values for both param - first one text and second one is prio
                                    //http:localhost:[port]/api/notes/multipleQuery?priority=1&text=gym - with query params we can change the order, because we have key value pairs and we always know which value is for which key
        public ActionResult<List<Note>> FilterNotesByMultipleParams(string? text, int? priority)
        {
            try
            {
                if (string.IsNullOrEmpty(text) && priority == null)
                {
                    return BadRequest("You have to send at least one filter param");
                    // return Ok(StaticDb.Notes); //we can either return bad request or return all notes unfiltered
                }

                if (string.IsNullOrEmpty(text))
                {
                    //priority has a value
                    List<Note> filteredNotes = StaticDb.Notes.Where(x => (int)x.Priority == priority).ToList();
                    //List<Note> filteredNotes = StaticDb.Notes.Where(x => x.Priority == (PriorityEnum)priority).ToList();
                    return Ok(filteredNotes);
                }

                if (priority == null)
                {
                    //text has value
                    List<Note> notes = StaticDb.Notes.Where(x => x.Text.ToLower() == text.ToLower()).ToList();
                    return Ok(notes);
                }

                //if it did not enter any of the ifs above, that means that both text and priority were sent, so we filter by both
                List<Note> notesByBoth = StaticDb.Notes.Where(x => x.Text.ToLower() == text.ToLower() && (int)x.Priority == priority).ToList();
                return Ok(notesByBoth);
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        //if we want to read the User-Agent we need to store it in a variable with the same name, but User-Agent is not a valid name. That's why we need to use proper naming and then use Name="User=Agent"
        //if we only say [FromHeader] string userAgent - it will expect a key userAgent in the header
        [HttpGet("header")]
        public IActionResult GetHeader([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok(userAgent);
        }

        [HttpGet("lang")]
        public IActionResult GetHeaderLang([FromHeader] string lang)
        {
            return Ok(lang);
        }

        [HttpPost]
        public IActionResult PostNote([FromBody] Note note) //we need to tell it where to look for the JSON containing the data for this note object - look for it in the body of the request
        {
            try
            {
                if (note == null)
                {
                    return BadRequest("Note cannot be null");
                }

                if (string.IsNullOrEmpty(note.Text))
                {
                    return BadRequest("Text is a required property!");
                }

                if (note.Tags == null || note.Tags.Count == 0) {
                    return BadRequest("Tags are required");
                }

                StaticDb.Notes.Add(note);
                return StatusCode(StatusCodes.Status201Created, "Note created");

            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        [HttpPut("updateNote/{index}")]
        public IActionResult UpdateNote(int index, [FromBody] Tag tag)
        {
            try
            {
                if(index < 0)
                {
                    return BadRequest("Index cannot be negative");
                }

                if(index >= StaticDb.Notes.Count)
                {
                    return NotFound($"Note with index {index} does not exist");
                }

                if(tag == null)
                {
                    return BadRequest("Tag cannot be null");
                }

                Note noteForUpdate = StaticDb.Notes[index];
                if(noteForUpdate.Tags == null)
                {
                    noteForUpdate.Tags = new List<Tag>();
                }

                noteForUpdate.Tags.Add(tag);
                return StatusCode(StatusCodes.Status204NoContent, "Note updated");
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }

        [HttpPut("updateNoteQuery/{index}")]
        public IActionResult UpdateNoteFromQuery(int index, [FromQuery] Tag tag)
        {
            try
            {
                if (index < 0)
                {
                    return BadRequest("Index cannot be negative");
                }

                if (index >= StaticDb.Notes.Count)
                {
                    return NotFound($"Note with index {index} does not exist");
                }

                if (tag == null)
                {
                    return BadRequest("Tag cannot be null");
                }

                Note noteForUpdate = StaticDb.Notes[index];
                if (noteForUpdate.Tags == null)
                {
                    noteForUpdate.Tags = new List<Tag>();
                }

                noteForUpdate.Tags.Add(tag);
                return StatusCode(StatusCodes.Status204NoContent, "Note updated");
            }
            catch (Exception ex)
            {
                //an error occured but we still do not know concretly what happened
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured, contact your administrator");
            }
        }


    }
}
