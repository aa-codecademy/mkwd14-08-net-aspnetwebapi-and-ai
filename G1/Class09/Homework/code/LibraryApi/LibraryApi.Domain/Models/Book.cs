using LibraryApi.Domain.Enums;

namespace LibraryApi.Domain.Models;

// Configured with the FLUENT API, so this class carries no attributes.
public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int Year { get; set; }
    public int PageCount { get; set; }
    public Genre Genre { get; set; }
    // Foreign key.
    public int AuthorId { get; set; }
    // Navigation property. Null until we Include() it.
    public Author? Author { get; set; }
}
