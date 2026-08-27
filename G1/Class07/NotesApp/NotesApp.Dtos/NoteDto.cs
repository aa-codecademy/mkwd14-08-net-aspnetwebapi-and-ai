using NotesApp.Domain.Enums;

namespace NotesApp.Dtos
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Priority Priority { get; set; }
        public string UserFullName { get; set; }
        public List<TagDto> Tags { get; set; } = new();
    }
}
