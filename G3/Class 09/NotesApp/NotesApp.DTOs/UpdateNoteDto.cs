using NotesApp.Domain.Enums;

namespace NotesApp.DTOs
{
    public class UpdateNoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public PriorityEnum Priority { get; set; }
        public int UserId { get; set; } //FK - an id would be sent if on the client side we have a dropdown with users, or if we take the id of the loggedIn user
        public List<int> TagIds { get; set; } = new List<int>(); //we would have a list of ints if we have multiple select on the client side - the ids are the value of the option chosen

    }
}
