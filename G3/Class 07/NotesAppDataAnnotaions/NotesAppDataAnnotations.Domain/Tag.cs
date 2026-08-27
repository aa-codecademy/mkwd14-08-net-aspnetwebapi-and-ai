using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppDataAnnotations.Domain
{
    public class Tag : BaseEntity
    {
        public int NoteId { get; set; }

        [ForeignKey("NoteId")]
        public Note Note { get; set; }
    }
}
