using System;

namespace Model.DTOs.NoteDTOs.RequestDTO
{
    public class NoteRequestDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Reminder { get; set; }

        public string BackgroundColor { get; set; }

        public string Image { get; set; }
    }
}