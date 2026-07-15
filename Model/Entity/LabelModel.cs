using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Entity
{
    public  class LabelModel
    {
        [Key]
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        public int UserId { get; set; }

        public UserModel User { get; set; }

        public ICollection<NoteLabelModel> NoteLabels { get; set; }
    }
}
