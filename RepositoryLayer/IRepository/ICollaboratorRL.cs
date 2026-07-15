using Model.DTOs.CollaboratorDTOs.ResponseDTOs;

namespace RepositoryLayer.IRepository
{
    public interface ICollaboratorRL
    {
        Task<CollaboratorResponseDTO> AddCollaborator(int noteId,string email,int ownerId);

        Task<bool> RemoveCollaborator(
    int noteId,
    string email,
    int ownerId);


        Task<List<CollaboratorResponseDTO>> GetCollaborators(
    int noteId,
    int ownerId
);
    }




    
}