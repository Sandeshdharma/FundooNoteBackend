using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Text;

namespace Model.Entity
{
    public  class NotesModel
    {

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Remainder { get; set; }

        public string BackgroundColor { get; set; }

        public string Image { get; set; }

        public bool Pin { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public bool Trash { get; set; }

        public bool Archive { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public UserModel UserModel { get; set; }

        //public ICollection<CollaboratorModel>Collaborators { get; set; }

        public ICollection<NoteLabelModel> NoteLabels { get; set; }





    }
}
