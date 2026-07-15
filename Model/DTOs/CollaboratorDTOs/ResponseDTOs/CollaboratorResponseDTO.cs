using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs.CollaboratorDTOs.ResponseDTOs
{
    public class CollaboratorResponseDTO
    {
        public int CollaboratorId { get; set; }

        public int NoteId { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}