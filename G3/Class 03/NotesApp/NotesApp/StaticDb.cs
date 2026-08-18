using NotesApp.Models;
using NotesApp.Models.Enums;

namespace NotesApp
{
    public static class StaticDb
    {
        public static List<Note> Notes = new List<Note>()
        {
            new Note()
            {
                Text = "Do the homework",
                Priority = PriorityEnum.Medium,
                Tags = new List<Tag>() {
                new Tag() { Name = "Homework", Color ="Blue"},
                new Tag() { Name = "Avenga Academy", Color ="Red"}
                }
            },

             new Note()
            {
                Text = "Drink more water",
                Priority = PriorityEnum.High,
                Tags = new List<Tag>() {
                new Tag() { Name = "Health", Color ="Green"},
                }
            },
              new Note()
            {
                Text = "Go to the gym",
                Priority = PriorityEnum.Low,
                Tags = new List<Tag>() {
                new Tag() { Name = "Health", Color ="Green"},
                new Tag() { Name = "Exercise", Color ="Black"},
                }
            }
        };
    }
}
