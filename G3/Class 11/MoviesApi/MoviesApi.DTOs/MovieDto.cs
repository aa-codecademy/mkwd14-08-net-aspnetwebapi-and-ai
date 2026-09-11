using MoviesApi.Domain.Enums;

namespace MoviesApi.DTOs
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
        public string? Image {  get; set; }
    }
}
