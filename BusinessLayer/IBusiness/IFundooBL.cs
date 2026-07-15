using Model.DTOs.NoteDTOs.RequestDTO;
using Model.DTOs.NoteDTOs.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.IBusiness
{
    public interface IFundooBL
    {

        //create  Note

        Task<NoteResponseDTO> CreateNote(NoteRequestDTO note, int userId);

        Task<List<NoteResponseDTO>> GetAllActiveNotes(int userId);
        Task<List<NoteResponseDTO>> GetArchivedNotes(int userId);

        Task<List<NoteResponseDTO>> GetTrashedNotes(int userId);

        Task<List<NoteResponseDTO>> SearchNotes(string keyword , int userId);

        Task<NoteResponseDTO> UpdateNote(int noteId, NoteRequestDTO note , int userId);

        Task<bool> DeleteForever(int noteId, int userId);


        Task<bool> TogglePin(int noteId, int userId);

        Task<bool> ToggleArchive(int noteId, int userId);

        Task<bool> ToggleTrash(int noteId ,int  userID);

    }
}
