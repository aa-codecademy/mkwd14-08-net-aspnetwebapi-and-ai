using NotesApp.Domain.Models;
using NotesApp.Domain.Enums;

namespace NotesApp.DataAccess.Data
{
    public static class StaticDb
    {

        public static List<User> Users = new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "Petko",
                LastName = "Petkovski",
                Username = "p.petkovski"
            },

            new User
            {
                Id = 2,
                FirstName = "Nikola",
                LastName = "Nikolovski",
                Username = "n.nikolovski"
            },

            new User
            {
                Id = 3,
                FirstName = "Stefan",
                LastName = "Stefanovski",
                Username = "s.stefanovski"
            },
        };

        public static List<Tag> Tags = new List<Tag>()
        {
            new Tag {Id = 1, Name = "Homework", Color = "blue"},
            new Tag {Id = 2, Name = "Avenga", Color = "orange"},
            new Tag {Id = 3, Name = "Exercise", Color = "green"},
            new Tag {Id = 4, Name = "Health", Color = "yellow"},
            new Tag {Id = 5, Name = "Urgent", Color = "red"},
        };

        public static List<Note> Notes = new List<Note>()
        {
            new Note
            {
                Id = 1,
                Text = "Do your homework",
                Priority = PriorityEnum.Medium,
                UserId = 1,
                User = Users[0],
                Tags = new List<Tag> {Tags[0], Tags[1] }
            },

            new Note
            {
                Id = 2,
                Text = "Drink water",
                Priority = PriorityEnum.High,
                UserId = 1,
                User = Users[0],
                Tags = new List<Tag> {Tags[3] }
            },

            new Note
            {
                Id = 3,
                Text = "Go to the gym",
                Priority = PriorityEnum.Low,
                UserId = 2,
                User = Users[1],
                Tags = new List<Tag> {Tags[2], Tags[3] }
            }
        };
    }
}
