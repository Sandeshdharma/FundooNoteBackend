using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs.CollaboratorDTOs.RequestDTOs
{
    public class AddCollaboratorRequestDTO
    {
        public int NoteId { get; set; }

        public string Email { get; set; }
    }
}
