using Class03.NotesAndTagsApp.Models.Enums;

namespace Class03.NotesAndTagsApp.Models;

public class Note
{
    public string Text { get; set; }
    public Priority Priority { get; set; }
    public List<Tag> Tags { get; set; }
}
