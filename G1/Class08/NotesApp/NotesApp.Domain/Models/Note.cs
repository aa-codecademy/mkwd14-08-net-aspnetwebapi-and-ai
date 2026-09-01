using NotesApp.Domain.Enums;

namespace NotesApp.Domain.Models;

// Configured with the FLUENT API, so this class has no attributes and no EF Core using - it is a plain C# class describing a note.
public class Note : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    // Foreign key: how the database points at the owner.
    public int? UserId { get; set; }
    // Navigation property: how an object accesses its related data. Null until we Include() it.
    public User? User { get; set; }
    public List<Tag> Tags { get; set; } = new();
}

