using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Dtos;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                UserDto userDto = await _authService.RegisterAsync(registerDto);

                return StatusCode(StatusCodes.Status201Created, userDto);
            }
            catch (UserDataException ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (Exception)
            {
                return Problem(
                   detail: "An error occurred, please contact the administrator.",
                   statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
