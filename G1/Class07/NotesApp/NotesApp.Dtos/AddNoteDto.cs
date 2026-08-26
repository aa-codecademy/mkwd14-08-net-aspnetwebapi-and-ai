using NotesApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dtos
{
    public class AddNoteDto
    {
        public string Text { get; set; }
        public Priority Priority { get; set; }
        public int UserId { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}
