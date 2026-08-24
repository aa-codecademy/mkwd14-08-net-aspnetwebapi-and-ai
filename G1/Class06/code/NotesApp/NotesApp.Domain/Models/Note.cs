using NotesApp.Domain.Enums;

namespace NotesApp.Domain.Models;

public class Note : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public List<Tag> Tags { get; set; } = new();
}

