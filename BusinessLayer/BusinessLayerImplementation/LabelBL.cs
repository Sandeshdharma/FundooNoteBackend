using BusinessLayer.IBusiness;
using Microsoft.EntityFrameworkCore;
using Model.DTOs.LabelDTOs.RequestDTOs;
using Model.DTOs.LabelDTOs.ResponseDTOs;
using RepositoryLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BusinessLayerImplementation
{
    public class LabelBL : ILabelBL
    {

        private readonly ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }


        public async Task<LabelResponseDTO> CreateLabel(
          CreateLabelDTO request,
          int userId)
        {

            return await labelRL.CreateLabel(
              request,
              userId);

        }
        public async Task<List<LabelResponseDTO>> GetAllLabels(int userId)
        {
            return await labelRL.GetAllLabels(userId);
        }

        public async Task<LabelResponseDTO> UpdateLabel(int labelId, string labelName, int userId)
        {
            return await labelRL.UpdateLabel(labelId, labelName, userId);
        }

        public async Task<bool> DeleteLabel(int labelId, int userId)
        {
            return await labelRL.DeleteLabel(labelId, userId);
        }

        public async Task<bool> AddLabelToNote(int noteId, int labelId, int userId)
        {
            return await labelRL.AddLabelToNote(noteId, labelId, userId);
        }

        public async Task<bool> RemoveLabelFromNote(int noteId, int labelId, int userId)
        {
            return await labelRL.RemoveLabelFromNote(noteId, labelId, userId);
        }
    }
}
