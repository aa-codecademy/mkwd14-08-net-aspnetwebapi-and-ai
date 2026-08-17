using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Data;

/// <summary>
/// Our fake database for this class.
///
/// It lives in memory, so every time we stop the application everything is lost
/// and we start again from this seed data. 
/// </summary>
internal static class StaticDb
{
    // A real database generates ids for us. Here we have to do it by hand.
    private static int _noteIdCounter = 3;

    public static int NextNoteId() => ++_noteIdCounter;

    public static List<User> Users { get; set; } = new List<User>
    {
        new User
        {
            Id = 1,
            FirstName = "Bob",
            LastName = "Bobsky",
            Username = "bob",
            Password = "SuperSecret123"
        },
        new User
        {
            Id = 2,
            FirstName = "Petko",
            LastName = "Petkovski",
            Username = "petko",
            Password = "AlsoSecret456"
        }
    };

    public static List<Tag> Tags { get; set; } = new List<Tag>
    {
        new Tag { Id = 1, Name = "Homework", Color = "cyan" },
        new Tag { Id = 2, Name = "Avenga", Color = "blue" },
        new Tag { Id = 3, Name = "Healthy", Color = "orange" },
        new Tag { Id = 4, Name = "Exercise", Color = "green" },
        new Tag { Id = 5, Name = "Urgent", Color = "red" }
    };

    public static List<Note> Notes { get; set; } = new List<Note>
    {
        new Note
        {
            Id = 1,
            Text = "Do Homework",
            Priority = Priority.High,
            UserId = 1,
            User = Users[0],
            Tags = new List<Tag> { Tags[0], Tags[1] }
        },
        new Note
        {
            Id = 2,
            Text = "Drink more water",
            Priority = Priority.Medium,
            UserId = 1,
            User = Users[0],
            Tags = new List<Tag> { Tags[2] }
        },
        new Note
        {
            Id = 3,
            Text = "Go to the gym",
            Priority = Priority.Low,
            UserId = 2,
            User = Users[1],
            Tags = new List<Tag> { Tags[3], Tags[4] }
        }
    };
}
