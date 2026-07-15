using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.IRepository
{
    public  interface IFundooRL
    {

        //create Notes

        Task<NotesModel> CreateNotes(NotesModel note , int userId);

        //read

        Task<List<NotesModel>> GetAllActiveNotes(int userId);
        Task<List<NotesModel>> GetArchivedNotes(int userId);

        Task<List<NotesModel>> GetTrashedNotes(int userId);

        Task<List<NotesModel>> SearchNotes(string keyword, int userId);
        
        //update

        Task<NotesModel> UpdateNote(int noteId, NotesModel note, int userId);

        //delet

        // Soft Delete (Move to Trash)
        Task<bool> ToggleTrash(int noteId, int userId);

        // Permanent Delete
        Task<bool> DeleteForever(int noteId, int userId);

        // Status
        Task<bool> TogglePin(int noteId, int userId);

        Task<bool> ToggleArchive(int noteId, int userId);

        //collaborator

        //Task<bool> AddCollaborator(int noteId, int  collaboratorId);

        //Task<bool> RemoveCollaborator(int noteId, int CollaboratorId);








    }
}
