using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }



        [Required]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Forget Password Fields

        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public ICollection<NotesModel> Notes { get; set; }

        //public ICollection<CollaboratorModel> Collaborators { get; set; }

        public ICollection<LabelModel> Labels { get; set; }
    }
}