using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Entity
{
    public  class NoteLabelModel
    {

        [Key]
        public int Id { get; set; }

        public int NoteId { get; set; }

        public int LabelId { get; set; }

        public NotesModel Note { get; set; }

        public LabelModel Label { get; set; }
    }
}
