using Microsoft.EntityFrameworkCore;
using Model.DTOs.LabelDTOs.RequestDTOs;
using Model.DTOs.LabelDTOs.ResponseDTOs;
using Model.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;

namespace RepositoryLayer.RepositoryImplementation
{
    public class LabelRL : ILabelRL
    {
        private readonly NotesContext context;

        public LabelRL(NotesContext context)
        {
            this.context = context;
        }

        public async Task<LabelResponseDTO> CreateLabel(
            CreateLabelDTO request,
            int userId)
        {
            var existingLabel = await context.Labels
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.LabelName == request.LabelName);

            if (existingLabel != null)
            {
                return null;
            }

            LabelModel label = new LabelModel()
            {
                LabelName = request.LabelName,
                UserId = userId
            };

            await context.Labels.AddAsync(label);

            await context.SaveChangesAsync();

            return new LabelResponseDTO()
            {
                LabelId = label.LabelId,
                LabelName = label.LabelName,
                UserId = label.UserId
            };


        }

        public async Task<List<LabelResponseDTO>> GetAllLabels(int userId)
        {
            var labels = await context.Labels
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return labels.Select(x => new LabelResponseDTO
            {
                LabelId = x.LabelId,
                LabelName = x.LabelName,
                UserId = x.UserId
            }).ToList();
        }

        public async Task<LabelResponseDTO> UpdateLabel(int labelId, string labelName, int userId)
        {
            var label = await context.Labels.FirstOrDefaultAsync(x => x.LabelId == labelId && x.UserId == userId);
            if (label == null) return null;

            label.LabelName = labelName;
            await context.SaveChangesAsync();

            return new LabelResponseDTO
            {
                LabelId = label.LabelId,
                LabelName = label.LabelName,
                UserId = label.UserId
            };
        }

        public async Task<bool> DeleteLabel(int labelId, int userId)
        {
            var label = await context.Labels.FirstOrDefaultAsync(x => x.LabelId == labelId && x.UserId == userId);
            if (label == null) return false;

            var associatedNoteLabels = await context.NoteLabels.Where(nl => nl.LabelId == labelId).ToListAsync();
            context.NoteLabels.RemoveRange(associatedNoteLabels);

            context.Labels.Remove(label);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddLabelToNote(int noteId, int labelId, int userId)
        {
            var note = await context.NotesTbl.FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);
            var label = await context.Labels.FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);

            if (note == null || label == null) return false;

            var existingLink = await context.NoteLabels.FirstOrDefaultAsync(nl => nl.NoteId == noteId && nl.LabelId == labelId);
            if (existingLink != null) return false;

            var noteLabel = new NoteLabelModel
            {
                NoteId = noteId,
                LabelId = labelId
            };

            await context.NoteLabels.AddAsync(noteLabel);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveLabelFromNote(int noteId, int labelId, int userId)
        {
            var note = await context.NotesTbl.FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);
            var label = await context.Labels.FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);

            if (note == null || label == null) return false;

            var link = await context.NoteLabels.FirstOrDefaultAsync(nl => nl.NoteId == noteId && nl.LabelId == labelId);
            if (link == null) return false;

            context.NoteLabels.Remove(link);
            await context.SaveChangesAsync();
            return true;
        }

    }
}