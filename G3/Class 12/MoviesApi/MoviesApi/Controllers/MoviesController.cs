using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Domain.Enums;
using MoviesApi.DTOs;
using MoviesApi.Services.Interfaces;


namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase  //http://localhost:[port]/api/Movies
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet] //http://localhost:[port]/api/Movies
        [Authorize]
        public ActionResult<List<MovieDto>> GetAll()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    throw new ArgumentNullException("Identity is null");
                }

                if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                {
                    throw new Exception("User id does not exist in claims");
                }

                var movies = _movieService.GetAllMovies(userId);

                return Ok(movies);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filter")]
        [Authorize]
        public ActionResult<List<MovieDto>> FilterMovies(int? year, GenreEnum genre)
        {
            try
            {
                //var identity = HttpContext.User.Identity as ClaimsIdentity;
                //if (identity == null)
                //{
                //    throw new ArgumentNullException("Identity is null");
                //}

                //if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                //{
                //    throw new Exception("User id does not exist in claims");
                //}

                return Ok(_movieService.FilterMovies(year, genre));
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteById(int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("add")]
        [Authorize]
        public IActionResult AddMovie([FromForm] AddMovieDto addMovieDto)
        {
            try
            {
                //we want to get the id of the logged in user - we get that from our claims
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    throw new ArgumentNullException("Identity is null");
                }

                if (!int.TryParse(identity.FindFirst("id")?.Value, out int userId))
                {
                    throw new Exception("User id does not exist in claims");
                }

                _movieService.AddMovie(addMovieDto, userId);
                return StatusCode(StatusCodes.Status201Created, "Movie was created");
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
