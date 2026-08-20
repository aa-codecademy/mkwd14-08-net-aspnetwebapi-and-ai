using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DTOs;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        [HttpGet] //http:localhost:[port]/api/notes
        public ActionResult<List<NoteDto> GetAll() //the controller must return dtos instead of domain objects
        {
            try
            {
                //TODO add call to the service

            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please contact your administrator");
            }
        }

    }
}
