using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MoviesApi.Models;
using MoviesApi.Models.DTOs;
using MoviesApi.Models.Enums;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase  //http://localhost:[port]/api/Movies
    {

        [HttpGet] //http://localhost:[port]/api/Movies
        public ActionResult<List<MovieDto>> GetAll()
        {
            try
            {
                //1. we need to get the movies from db
                var moviesDb = StaticDb.Movies;

                //validation
                if (moviesDb == null)
                {
                    return Ok(new List<MovieDto>()); //we haven't added any movies yet, so we return an empty list
                }

                var moviesDto = moviesDb.Select(x => new MovieDto //we need to map each item that was returned from db into a dto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Year = x.Year,
                    Genre = x.Genre
                }).ToList();

                return Ok(moviesDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //we must send the path param because it is a part of the main route - it differences this endpoint from the others
        //if we omit the route param the route would hit the GetAll endpoint instead of this one (//http://localhost:[port]/api/Movies)
        [HttpGet("{id}")] //http://localhost:[port]/api/Movies/1
        public ActionResult<MovieDto> GetById(int id)
        {
            try
            {
                //validation 
                if (id <= 0) //if the id is a negative value, we don't need to look in the db, we already know that it does not exist
                {
                    return BadRequest("Bad request, the id cannot have a negative value!");
                }

                //get the movie from the db
                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);

                if (movieDb == null)
                {
                    return NotFound($"Movie with id {id} was not found");
                }

                //map 
                var movieDto = new MovieDto
                {
                    Title = movieDb.Title,
                    Description = movieDb.Description,
                    Year = movieDb.Year,
                    Genre = movieDb.Genre
                };

                return Ok(movieDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //here it is totaly valid (from routing perspective) if we don't send the id, because the query string is not a part of the "main" route, and if we omit the id, this endpoint will still be hit
        [HttpGet("query")] //http://localhost:[port]/api/Movies/query - this is the route if we don't send the id
                           //http://localhost:[port]/api/Movies/query?id=1 - this is the route if we send the id as a query param
        public ActionResult<MovieDto> GetByQueryId(int? id)
        {
            try
            {
                //just because it is valid for the routing to not send the id, that does not mean that is valid for the business logic in this case
                //so here, we need to make a validation
                if (id == null)
                {
                    return BadRequest("Id cannot be null");
                }
                //validation 
                if (id <= 0) //if the id is a negative value, we don't need to look in the db, we already know that it does not exist
                {
                    return BadRequest("Bad request, the id cannot have a negative value!");
                }

                //get the movie from the db
                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);

                if (movieDb == null)
                {
                    return NotFound($"Movie with id {id} was not found");
                }

                //map 
                var movieDto = new MovieDto
                {
                    Title = movieDb.Title,
                    Description = movieDb.Description,
                    Year = movieDb.Year,
                    Genre = movieDb.Genre
                };

                return Ok(movieDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filter")] //http://localhost:[port]/api/Movies/filter - the route if we don't send a query param
                            //http://localhost:[port]/api/Movies/filter?genre=1 - if we send only the genre as query param
                            //http://localhost:[port]/api/Movies/filter?year=2024 - if we send only the year as query param
                            //http://localhost:[port]/api/Movies/filter?genre=1&year=2024 - if we send both, first the genre then the year
                            //http://localhost:[port]/api/Movies/filter?year=2024&genre=1 - if we send both, first the year then the genre
        public ActionResult<List<MovieDto>> FilterMovies(int? genre, int? year)
        {
            try
            {
                //if the user does not send anything as filter (omits both query params) we can return all items
                if (!genre.HasValue && !year.HasValue)
                {
                    var moviesDb = StaticDb.Movies;

                    var moviesDto = moviesDb.Select(x => new MovieDto
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Year = x.Year,
                        Genre = x.Genre,
                    }).ToList();

                    return Ok(moviesDto);
                }

                //if the genre was sent - we need to validate that the genre is in the correct range - that it exists
                if (genre.HasValue)
                {
                    //var enumValues = Enum.GetValues(typeof(GenreEnum)) //returns array of the value as type Enum
                    //    .Cast<GenreEnum>() // we need our specific type of enum - GenreEnum, not the base Enum type
                    //    .Select(genre => (int)genre) //1,2,3...5
                    //    .ToList();

                    //in the background this checks if the value that was sent for genre exists in our GenreEnum
                    if (!Enum.IsDefined(typeof(GenreEnum), genre))
                    {
                        return NotFound($"The genre with id {genre} was not found");
                    }
                }

                //if the genre was sent, but the year was not sent
                if (year == null)
                {
                    List<Movie> moviesDbByGenre = StaticDb.Movies.Where(x => (int)x.Genre == genre.Value).ToList();

                    var moviesGenreDto = moviesDbByGenre.Select(x => new MovieDto
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Genre = x.Genre,
                        Year = x.Year
                    }).ToList();

                    return Ok(moviesGenreDto);
                }

                //if the year was sent, but the genre was not sent

                if (genre == null)
                {
                    List<Movie> moviesDbByYear = StaticDb.Movies.Where(x => x.Year == year).ToList();
                    var moviesYearDto = moviesDbByYear.Select(x => new MovieDto
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Genre = x.Genre,
                        Year = x.Year
                    }).ToList();

                    return Ok(moviesYearDto);
                }

                List<Movie> moviesDbFiltered = StaticDb.Movies.Where(x => (int)x.Genre == genre.Value && x.Year == year).ToList();

                var moviesFilteredDto = moviesDbFiltered.Select(x => new MovieDto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Genre = x.Genre,
                    Year = x.Year
                }).ToList();

                return Ok(moviesFilteredDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost] //http://localhost:[port]/api/Movies
        public IActionResult AddMovie([FromBody] AddMovieDto addMovieDto)
        {
            try
            {
                //validation
                if (addMovieDto == null)
                {
                    return BadRequest("Movie cannot be null");
                }

                if (string.IsNullOrEmpty(addMovieDto.Title))
                {
                    return BadRequest("Title is required");
                }

                //description is optional, BUT if description was sent it must have max 250 characters
                if (!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
                {
                    return BadRequest("Description must have max 250 characters");
                }
                //if the genre was sent - we need to validate that the genre is in the correct range - that it exists

                if (!Enum.IsDefined(typeof(GenreEnum), addMovieDto.Genre))
                {
                    return NotFound($"The genre with id {addMovieDto.Genre} was not found");
                }

                if (addMovieDto.Year <= 0)
                {
                    return BadRequest("Year cannot have a negative value or be 0");
                }

                //map the movieDto into a movie (domain model) object
                Movie newMovie = new Movie
                {
                    Id = StaticDb.Movies.LastOrDefault() != null ? StaticDb.Movies.LastOrDefault().Id + 1 : 1,
                    Title = addMovieDto.Title,
                    Description = addMovieDto.Description,
                    Genre = addMovieDto.Genre,
                    Year = addMovieDto.Year,
                };

                //add the movie to db
                StaticDb.Movies.Add(newMovie);
                return StatusCode(StatusCodes.Status201Created, "Movie was created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut] //http://localhost:[port]/api/Movies
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto updateMovieDto)
        {
            try
            {
                //validations
                if (updateMovieDto == null)
                {
                    return BadRequest("Movie cannot be null");
                }

                Movie movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == updateMovieDto.Id);
                if (movieDb == null)
                {
                    return NotFound($"Movie with id {updateMovieDto.Id} does not exist");

                }
                //the same rules (requirements) as we had for create

                if (string.IsNullOrEmpty(updateMovieDto.Title))
                {
                    return BadRequest("Title is required");
                }

                //description is optional, BUT if description was sent it must have max 250 characters
                if (!string.IsNullOrEmpty(updateMovieDto.Description) && updateMovieDto.Description.Length > 250)
                {
                    return BadRequest("Description must have max 250 characters");
                }
                //if the genre was sent - we need to validate that the genre is in the correct range - that it exists

                if (!Enum.IsDefined(typeof(GenreEnum), updateMovieDto.Genre))
                {
                    return NotFound($"The genre with id {updateMovieDto.Genre} was not found");
                }

                if (updateMovieDto.Year <= 0)
                {
                    return BadRequest("Year cannot have a negative value or be 0");
                }

                //map
                movieDb.Title = updateMovieDto.Title;
                movieDb.Description = updateMovieDto.Description;
                movieDb.Year = updateMovieDto.Year;
                movieDb.Genre = updateMovieDto.Genre;

                int movieIndex = StaticDb.Movies.FindIndex(x => x.Id == updateMovieDto.Id);
                StaticDb.Movies[movieIndex] = movieDb;

                return StatusCode(StatusCodes.Status204NoContent, "Movie was updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        //Route param
        [HttpDelete("{id}")] //http://localhost:[port]/api/Movies/1
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                //validation
                if (id <= 0)
                {
                    return BadRequest("Id cannot have a negative value");
                }

                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if (movieDb == null)
                {
                    return NotFound($"Movie with id {id} does not exist");
                }

                StaticDb.Movies.Remove(movieDb);
                return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //From body
        [HttpDelete] //http://localhost:[port]/api/Movies
        public IActionResult DeleteMovieFromBody([FromBody] int id)
        {
            try
            {
                //validation
                if (id <= 0)
                {
                    return BadRequest("Id cannot have a negative value");
                }

                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if (movieDb == null)
                {
                    return NotFound($"Movie with id {id} does not exist");
                }

                StaticDb.Movies.Remove(movieDb);
                return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //From query
        [HttpDelete("fromQuery")] //http://localhost:[port]/api/Movies/fromQuery
        //http://localhost:[port]/api/Movies/fromQuery?id=1
        public IActionResult DeleteMovieFromQuery(int? id)
        {
            try
            {
                //validation
                if(id == null)
                {
                    return BadRequest("Id cannot be null");
                }

                if (id <= 0)
                {
                    return BadRequest("Id cannot have a negative value");
                }

                var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
                if (movieDb == null)
                {
                    return NotFound($"Movie with id {id} does not exist");
                }

                StaticDb.Movies.Remove(movieDb);
                return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
