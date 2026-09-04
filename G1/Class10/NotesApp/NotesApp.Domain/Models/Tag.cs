using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Domain.Models;

[Table("Tag")]
[Index(nameof(Name), IsUnique = true)]
public class Tag : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    //[Column("HexColor")] // specify the column name in the database if you want it to be different from the property name. In this case, we are keeping it the same.
    public string Color { get; set; } = string.Empty;
}