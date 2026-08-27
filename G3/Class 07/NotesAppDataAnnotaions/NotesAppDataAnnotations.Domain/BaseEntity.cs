using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotesAppDataAnnotations.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //identity in db, the db will generate the value for the id, starting from 1 and incrementing by 1
        public int Id { get; set; }
    }
}
