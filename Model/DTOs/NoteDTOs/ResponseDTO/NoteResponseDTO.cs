using System;

namespace Model.DTOs.NoteDTOs.ResponseDTO
{
    public class NoteResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string BackgroundColor { get; set; }
        public string Image { get; set; }
        public bool Pin { get; set; }
        public bool Archive { get; set; }
        public bool Trash { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public int CreatorId { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string CreatorEmail { get; set; }
        public System.Collections.Generic.List<Model.DTOs.LabelDTOs.ResponseDTOs.LabelResponseDTO> Labels { get; set; }
    }
}