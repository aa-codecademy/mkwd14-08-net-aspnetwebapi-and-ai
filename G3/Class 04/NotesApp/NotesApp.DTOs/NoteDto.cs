using NotesApp.Domain.Enums;

namespace NotesApp.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public PriorityEnum Priority{ get; set;}
        public string UserFullName { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
