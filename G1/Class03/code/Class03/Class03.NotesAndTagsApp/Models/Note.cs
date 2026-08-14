namespace Class03.NotesAndTagsApp.Models;

public class Note
{
    public string Text { get; set; }
    public string Priority { get; set; }
    public List<Tag> Tags { get; set; }
}
