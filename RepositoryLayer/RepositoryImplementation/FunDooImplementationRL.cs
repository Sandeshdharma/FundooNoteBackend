using Model.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace RepositoryLayer.RepositoryImplementation
{
    public  class FunDooImplementationRL : IFundooRL
    {
         
        private readonly NotesContext context;
        public FunDooImplementationRL (NotesContext context)
        {
            this.context = context;
        }

        public async Task<NotesModel> CreateNotes(NotesModel note , int userId)
        {
            note.Created = DateTime.Now;
            note.Edited = DateTime.Now;
            note.UserId = userId;

            await context.NotesTbl.AddAsync(note);
            await context.SaveChangesAsync();  
            return note;
        }

        public async Task<List<NotesModel>> GetAllActiveNotes(int userId)
        {
            return await context.NotesTbl.Include(x => x.UserModel)
                .Include(x => x.NoteLabels).ThenInclude(nl => nl.Label)
                .Where(x =>
                    (x.UserId == userId ||
                     context.Collaborators.Any(c => c.NoteId == x.Id && c.UserId == userId)) &&
                    !x.Archive &&
                    !x.Trash)
                .ToListAsync();
        }

        public async Task<List<NotesModel>> GetTrashedNotes(int userId)
        {
            return await context.NotesTbl.Include(x => x.UserModel)
                .Include(x => x.NoteLabels).ThenInclude(nl => nl.Label)
                .Where(x =>x.UserId == userId && x.Trash).ToListAsync();
        }

        public async Task<List<NotesModel>> GetArchivedNotes(int userId)
        { 
            return await context.NotesTbl.Include(x => x.UserModel)
                .Include(x => x.NoteLabels).ThenInclude(nl => nl.Label)
                .Where(x =>x.UserId == userId && x.Archive).ToListAsync();
        }

        public async Task<List<NotesModel>> SearchNotes(string keyword, int userId)
        {
            return await context.NotesTbl.Include(x => x.UserModel)
                .Include(x => x.NoteLabels).ThenInclude(nl => nl.Label)
                .Where(x =>
                    (x.UserId == userId ||
                     context.Collaborators.Any(c => c.NoteId == x.Id && c.UserId == userId)) &&
                    (x.Title.Contains(keyword) || x.Description.Contains(keyword)))
                .ToListAsync();
        }

        public async Task<NotesModel> UpdateNote(int noteId,NotesModel note,int userId)
        {
            var existingNote = await context.NotesTbl.Include(x => x.UserModel)
                .FirstOrDefaultAsync(x => x.Id == noteId);

            if (existingNote == null)
            {
                return null;
            }

            bool isOwner = existingNote.UserId == userId;

            bool isCollaborator = await context.Collaborators
                .AnyAsync(x =>
                    x.NoteId == noteId &&
                    x.UserId == userId);

            if (!isOwner && !isCollaborator)
            {
                return null;
            }

            existingNote.Title = note.Title;

            existingNote.Description = note.Description;

            existingNote.Remainder = note.Remainder;

            existingNote.BackgroundColor = note.BackgroundColor;

            existingNote.Image = note.Image;

            existingNote.Edited = DateTime.Now;

            await context.SaveChangesAsync();

            return existingNote;
        }

        public async Task<bool> DeleteForever(int noteId, int userId)
        {
            var note = await context.NotesTbl
                .FirstOrDefaultAsync(x =>
                    x.Id == noteId &&
                    x.UserId == userId);

            if (note == null)
            {
                return false;
            }

            // Remove associated Collaborators
            var collaborators = context.Collaborators.Where(c => c.NoteId == noteId);
            context.Collaborators.RemoveRange(collaborators);

            // Remove associated NoteLabels
            var noteLabels = context.NoteLabels.Where(nl => nl.NoteId == noteId);
            context.NoteLabels.RemoveRange(noteLabels);

            context.NotesTbl.Remove(note);

            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> TogglePin(int noteId, int userId)
        {
            var note = await context.NotesTbl
                .FirstOrDefaultAsync(x =>
                    x.Id == noteId &&
                    x.UserId == userId);

            if (note == null)
            {
                return false;
            }

            note.Pin = !note.Pin;

            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ToggleArchive(int noteId, int userId)
        {
            var note = await context.NotesTbl
                .FirstOrDefaultAsync(x =>
                    x.Id == noteId &&
                    x.UserId == userId);

            if (note == null)
            {
                return false;
            }

            note.Archive = !note.Archive;

            note.Edited = DateTime.Now;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleTrash(int noteId, int userId)
        {
            var note = await context.NotesTbl
                .FirstOrDefaultAsync(x =>
                    x.Id == noteId &&
                    x.UserId == userId);

            if (note == null)
            {
                return false;
            }

            note.Trash = !note.Trash;

            note.Edited = DateTime.Now;

            await context.SaveChangesAsync();

            return true;
        }



















    }
}
