using Microsoft.EntityFrameworkCore;
using MoviesApi.DataAccess.Interfaces;
using MoviesApi.Domain.Enums;
using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess.Implementations
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public void Add(Movie entity)
        {
            _context.Movies.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Movie entity)
        {
            _context.Movies.Remove(entity);
            _context.SaveChanges();
        }

        public List<Movie> FilterMovies(int? year, GenreEnum? genre)
        {
            //if both year and genre are null - return all
            if(year == null || genre == null)
            {
                return _context.Movies.Include(x => x.User).ToList(); //GetAll
            }

            //if year is null and genre is not null - filter by genre
            if(year == null)
            {
                List<Movie> moviesDb = _context.Movies.Include(x => x.User).Where(x => x.Genre == genre).ToList();
                return moviesDb;
            }

            //if genre is null and year is not null - filter by year
            if(genre == null)
            {
                List<Movie> moviesDb = _context.Movies.Include(x => x.User).Where(x => x.Year == year).ToList();
                return moviesDb;
            }

            //if both are not null
            List<Movie> movies = _context.Movies.Include(x => x.User).Where(x => x.Genre == genre && x.Year == year).ToList();
            return movies;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.Include(x => x.User).ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.Include(x => x.User).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Movie entity)
        {
            _context.Movies.Update(entity);
            _context.SaveChanges();
        }
    }
}
