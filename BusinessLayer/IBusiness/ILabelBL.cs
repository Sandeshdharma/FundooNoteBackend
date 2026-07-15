using Model.DTOs.LabelDTOs.RequestDTOs;
using Model.DTOs.LabelDTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.IBusiness
{
    public  interface ILabelBL
    {

        Task<LabelResponseDTO> CreateLabel(CreateLabelDTO request, int userId);

        Task<List<LabelResponseDTO>> GetAllLabels(int userId);

        Task<LabelResponseDTO> UpdateLabel(int labelId, string labelName, int userId);
        Task<bool> DeleteLabel(int labelId, int userId);
        Task<bool> AddLabelToNote(int noteId, int labelId, int userId);
        Task<bool> RemoveLabelFromNote(int noteId, int labelId, int userId);
    }
}
