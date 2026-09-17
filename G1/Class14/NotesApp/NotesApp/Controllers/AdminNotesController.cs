using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Controllers
{
    [Route("api/admin/notes")]
    [ApiController]
    [Authorize]
    // How to restrict access to users in the "Admin" role only ???
    public class AdminNotesController : ControllerBase
    {
    }
}
