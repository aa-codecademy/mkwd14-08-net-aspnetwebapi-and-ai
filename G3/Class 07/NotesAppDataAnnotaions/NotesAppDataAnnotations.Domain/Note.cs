using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotesAppDataAnnotations.Domain.Enums;

namespace NotesAppDataAnnotations.Domain
{
    [Table("Notes")]
    public class Note : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Text { get; set; }
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } //part of the 1-M relationship with note. Here we are telling EF that the User property is the table that we connect to the note and we join by the foreign key UserId
        public PriorityEnum Priority { get; set; }

        [NotMapped] //we dont want this property to be mapped as a column in the table Notes
        public int NoteCount { get; set; }

        [InverseProperty("Note")] //the other end of the relationship - corresponds to the User property in Note class
        public List<Tag> Tags { get; set; }
    }
}
