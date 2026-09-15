using Microsoft.AspNetCore.Http;
using MoviesApi.Domain.Models;
using MoviesApi.DTOs;

namespace MoviesApi.Mappers
{
    public static class MovieMapper
    {
        //from domain to dto
        public static MovieDto ToMovieDto(this Movie movie) //extension method
        {
            return new MovieDto
            {
                Year = movie.Year,
                Description = movie.Description,
                Title = movie.Title,
                Genre = movie.Genre,
                Image = movie.Image != null ? Convert.ToBase64String(movie.Image) : null,
            };
        }

        //from addMovieDto to domain model
        public static Movie ToMovie(this AddMovieDto dto) {

            return new Movie
            {
                Year = dto.Year,
                Description = dto.Description,
                Title = dto.Title,
                Genre = dto.Genre,
                Image = dto.Image != null ? ConvertToBytes(dto.Image) : null,
            };
        }

        //we need to convert our image from IFormFile to byte[]
        private static byte[] ConvertToBytes(IFormFile image)
        {
            using var memoryStream = new MemoryStream();    
            image.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
