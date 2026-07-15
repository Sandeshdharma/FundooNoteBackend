using Microsoft.EntityFrameworkCore;
using Model.DTOs.CollaboratorDTOs.ResponseDTOs;
using Model.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;

namespace RepositoryLayer.RepositoryImplementation
{
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly NotesContext context;

        public CollaboratorRL(NotesContext context)
        {
            this.context = context;
        }

        public async Task<CollaboratorResponseDTO> AddCollaborator(
            int noteId,
            string email,
            int ownerId)
        {
            // Check Note belongs to logged-in user or they are a collaborator
            var note = await context.NotesTbl
                .FirstOrDefaultAsync(x => x.Id == noteId);

            if (note == null)
            {
                return null;
            }

            bool hasAccess = note.UserId == ownerId || await context.Collaborators.AnyAsync(c => c.NoteId == noteId && c.UserId == ownerId);
            if (!hasAccess)
            {
                return null;
            }

            // Check user exists
            var user = await context.UserTbl
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            // Check duplicate collaborator
            var alreadyExists = await context.Collaborators
                .FirstOrDefaultAsync(x => x.NoteId == noteId &&
                                          x.Email == email);

            if (alreadyExists != null)
            {
                return null;
            }

            var collaborator = new CollaboratorModel
            {
                NoteId = noteId,
                UserId = user.UserId,
                Email = email
            };

            await context.Collaborators.AddAsync(collaborator);
            await context.SaveChangesAsync();

            return new CollaboratorResponseDTO
            {
                CollaboratorId = collaborator.CollaboratorId,
                NoteId = collaborator.NoteId,
                UserId = collaborator.UserId,
                Email = collaborator.Email
            };
        }




        public async Task<bool> RemoveCollaborator(int noteId,string email,int ownerId)
        {
            var note = await context.NotesTbl.FirstOrDefaultAsync(x => x.Id == noteId);

            if (note == null)
            {
                return false;
            }

            var requester = await context.UserTbl.FirstOrDefaultAsync(u => u.UserId == ownerId);
            bool isOwner = note.UserId == ownerId;
            bool isSelfRemove = requester != null && requester.Email == email;

            if (!isOwner && !isSelfRemove)
            {
                return false;
            }

            var collaborator =
                await context.Collaborators
                .FirstOrDefaultAsync(x =>
                    x.NoteId == noteId &&
                    x.Email == email);

            if (collaborator == null)
            {
                return false;
            }

            context.Collaborators.Remove(collaborator);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CollaboratorResponseDTO>> GetCollaborators(
    int noteId,
    int ownerId)
        {
            var hasAccess = await context.NotesTbl.AnyAsync(n => n.Id == noteId && n.UserId == ownerId) ||
                            await context.Collaborators.AnyAsync(c => c.NoteId == noteId && c.UserId == ownerId);

            if (!hasAccess)
            {
                return null;
            }

            var collaborators = await context.Collaborators

                .Where(c => c.NoteId == noteId)

                .Join(

                    context.UserTbl,

                    collaborator => collaborator.UserId,

                    user => user.UserId,

                    (collaborator, user) =>

                    new CollaboratorResponseDTO
                    {
                        CollaboratorId = collaborator.CollaboratorId,

                        NoteId = collaborator.NoteId,

                        UserId = collaborator.UserId,

                        Email = user.Email,

                        FirstName = user.FirstName,

                        LastName = user.LastName
                    })

                .ToListAsync();

            return collaborators;
        }




    }
}
