using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Domain.Models;

// Configured with DATA ANNOTATIONS. Compare with Book, which uses the Fluent API.
[Table("Author")]
[Index(nameof(LastName))]
public class Author : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    [NotMapped] // Computed, never stored.
    public string FullName => $"{FirstName} {LastName}";
    public List<Book> Books { get; set; } = new();
}
