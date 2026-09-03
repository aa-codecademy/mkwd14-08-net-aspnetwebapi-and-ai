using LibraryApi.Domain.Enums;

namespace LibraryApi.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int Year { get; set; }
    public int PageCount { get; set; }
    public Genre Genre { get; set; }
    public string AuthorFullName { get; set; } = string.Empty;
}
