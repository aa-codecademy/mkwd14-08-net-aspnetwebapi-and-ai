using Microsoft.AspNetCore.Http;
using MoviesApi.Domain.Enums;

namespace MoviesApi.DTOs
{
    public class AddMovieDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public GenreEnum Genre { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
