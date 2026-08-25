using MoviesApi.Models.Enums;

namespace MoviesApi.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
    }
}
