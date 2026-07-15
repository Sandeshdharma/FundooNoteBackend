using System;
using System.Collections.Generic;
using System.Text;
using Model.DTOs.CollaboratorDTOs.ResponseDTOs;

namespace BusinessLayer.IBusiness
{
    public  interface ICollaboratorBL
    {
        Task<CollaboratorResponseDTO> AddCollaborator(int noteId, string email, int ownerId);

        Task<bool> RemoveCollaborator(int noteId,string email,int ownerId);

        Task<List<CollaboratorResponseDTO>>GetCollaborators(int noteId,int ownerId);


    }
}
