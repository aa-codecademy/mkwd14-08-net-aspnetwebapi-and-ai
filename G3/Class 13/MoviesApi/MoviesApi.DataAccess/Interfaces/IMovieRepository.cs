using MoviesApi.Domain.Enums;
using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        List<Movie> FilterMovies(int? year, GenreEnum? genre);
    }
}
