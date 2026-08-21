using NotesApp.Domain.Enums;

namespace NotesApp.Domain.Models
{
    public class Note : BaseEntity
    {
        public string Text { get; set; }
        public PriorityEnum Priority { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>(); //to avoid the null value
        public int UserId { get; set; } //FK
        public User User { get; set; } //we need the whole object to be able to access all the data in just one db call
    }
}
