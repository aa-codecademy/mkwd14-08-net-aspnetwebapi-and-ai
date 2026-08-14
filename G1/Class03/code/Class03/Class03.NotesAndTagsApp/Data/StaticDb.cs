using Class03.NotesAndTagsApp.Models;

namespace Class03.NotesAndTagsApp.Data;

public static class StaticDb
{
    public static List<Note> Notes { get; set; } = new List<Note>()
    {
        new Note()
        {
            Text = "This is my first note",
            Priority = "High",
            Tags = new List<Tag>()
            {
                new Tag() { Name = "Work", Color = "Red" },
                new Tag() { Name = "Personal", Color = "Blue" }
            }
        },
        new Note()
        {
            Text = "This is my second note",
            Priority = "Medium",
            Tags = new List<Tag>()
            {
                new Tag() { Name = "Work", Color = "Red" },
                new Tag() { Name = "Personal", Color = "Blue" }
            }
        },
        new Note()
        {
            Text = "This is my third note",
            Priority = "Low",
            Tags = new List<Tag>()
            {
                new Tag() { Name = "Work", Color = "Red" },
                new Tag() { Name = "Personal", Color = "Blue" }
            }
        }
    };
}
