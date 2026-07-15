using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity
{
    public class CollaboratorModel
    {
        [Key]
        public int CollaboratorId { get; set; }

        public int NoteId { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        [ForeignKey("NoteId")]
        public virtual NotesModel Note { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
}