using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Dtos;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Controllers
{
    [Authorize] // This attribute indicates that all actions in this controller require authentication by default
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous] // This attribute allows unauthenticated access to this specific action
        // We use it only for endpoints that should be accessible without authentication, such as registration and login.
        // POST: /api/auth/register
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

        [AllowAnonymous]
        // POST: /api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                LoginResponseDto response = await _authService.LoginAsync(loginDto);

                return Ok(response);
            }
            catch (InvalidCredentialsException ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status401Unauthorized
                );
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
