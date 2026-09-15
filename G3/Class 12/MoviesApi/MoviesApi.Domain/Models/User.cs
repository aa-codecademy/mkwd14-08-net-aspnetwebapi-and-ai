using MoviesApi.Domain.Enums;

namespace MoviesApi.Domain.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }    
        public string Password { get; set; }    
        public string Firstname { get; set; }    
        public string Lastname { get; set; }    
        public GenreEnum FavouriteGenre { get; set; }    
        public List<Movie> Movies { get; set; }    
    }
}
