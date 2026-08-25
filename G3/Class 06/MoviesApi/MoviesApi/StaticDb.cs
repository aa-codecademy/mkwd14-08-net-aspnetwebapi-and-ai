using MoviesApi.Models;
using MoviesApi.Models.Enums;

namespace MoviesApi
{
    public static class StaticDb
    {
        public static List<Movie> Movies = new List<Movie>()
        {
            new Movie
            {
                Id = 1,
                Title = "Spiderman",
                Description = "Marvel",
                Genre = GenreEnum.SciFi,
                Year = 2026
            },

            new Movie
            {
                Id = 2,
                Title = "Harry Potter",
                Description = "Magic",
                Genre = GenreEnum.Drama,
                Year = 2001
            },
            new Movie
            {
                Id = 3,
                Title = "Bad boys",
                Description = "Action",
                Genre = GenreEnum.Action,
                Year = 2024
            },
        };
    }
}
