using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAppDataAnnotations.Domain
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [Column("Email")]
        public string Username { get; set; }

        [InverseProperty("User")] //the other end of the relationship - corresponds to the User property in Note class
        public List<Note> Notes { get; set; }
    }
}
