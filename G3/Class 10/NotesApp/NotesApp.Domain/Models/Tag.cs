namespace NotesApp.Domain.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public int NoteCount { get; set; }

        public List<Note> Notes { get; set; }   
    }
}
