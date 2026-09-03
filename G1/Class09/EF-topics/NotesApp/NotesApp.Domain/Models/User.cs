using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Domain.Models;

// Configured with DATA ANNOTATIONS: the table is described here, on the class.
// Compare with Note, which uses the Fluent API instead.

[Table("User")] // The table name in the database will be "User".
[Index(nameof(Username), IsUnique = true)] // Enforce uniqueness of the Username column in the database.
public class User : BaseEntity
{
    [Required] // this property is required (not null) in the database.
    [MaxLength(100)] // the maximum length of this string in the database is 100 characters. It is nvarchar(100) in SQL Server, not nvarchar(max).
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    [NotMapped] // this property is not stored in the database, it is computed from other properties.
    public string FullName => $"{FirstName} {LastName}";
    public List<Note> Notes { get; set; } = new();
}

