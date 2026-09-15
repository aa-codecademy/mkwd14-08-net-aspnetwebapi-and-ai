using MoviesApi.Domain.Enums;
using MoviesApi.DTOs;

namespace MoviesApi.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies(int userId);
        List<MovieDto> FilterMovies(int? year, GenreEnum? genre);
        void DeleteMovie(int id);
        void AddMovie(AddMovieDto addMovieDto, int userId); //the userId is the id of the logged in user
    }
}
