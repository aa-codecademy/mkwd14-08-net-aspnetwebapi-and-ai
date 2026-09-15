using Microsoft.Identity.Client;
using MoviesApi.DataAccess.Interfaces;
using MoviesApi.Domain.Enums;
using MoviesApi.Domain.Models;
using MoviesApi.DTOs;
using MoviesApi.Mappers;
using MoviesApi.Services.Interfaces;

namespace MoviesApi.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void AddMovie(AddMovieDto addMovieDto, int userId)
        {
            //validations
            if (addMovieDto == null)
            {
                throw new ArgumentNullException("Model cannot be null");
            }

            if (string.IsNullOrEmpty(addMovieDto.Title))
            {
                throw new ArgumentNullException("Title is a required field");
            }

            if (!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
            {
                throw new ArgumentException("Description cannot be longer than 250 characters");
            }

            if (!Enum.IsDefined(addMovieDto.Genre))
            {
                throw new ArgumentException("Invalid value for genre");
            }

            if(addMovieDto.Year < 0 || addMovieDto.Year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid value for year");
            }

            //map the addMovieDto to domain
            Movie newMovie = addMovieDto.ToMovie();
            newMovie.UserId = userId; //the logged in user

            _movieRepository.Add(newMovie);
        }

        public void DeleteMovie(int id)
        {
            //get by id and validation
            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new NullReferenceException($"Movie with id {id} was not found");
            }

            _movieRepository.Delete(movieDb);
        }

        public List<MovieDto> FilterMovies(int? year, GenreEnum? genre)
        {
            if (genre.HasValue)
            {
                if (!Enum.IsDefined(genre.Value))
                {
                    throw new ArgumentException("Invalid genre value");
                }
            }

            if (year.HasValue && (year < 0 || year > DateTime.Now.Year))
            {

                throw new ArgumentException("Invalid value for year");
            }

            return _movieRepository.FilterMovies(year, genre).Select(x => x.ToMovieDto()).ToList();
        }

        public List<MovieDto> GetAllMovies(int userId)
        {
            return _movieRepository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => x.ToMovieDto())//we are using a mapper to get a cleaner code and be able to reuse the mapping
                .ToList();
        }
    }
}
