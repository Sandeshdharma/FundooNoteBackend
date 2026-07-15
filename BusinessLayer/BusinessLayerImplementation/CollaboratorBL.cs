using BusinessLayer.IBusiness;
using Model.DTOs.CollaboratorDTOs.ResponseDTOs;
using RepositoryLayer.IRepository;

namespace BusinessLayer.BusinessImplementation
{
    public class CollaboratorBL : ICollaboratorBL
    {
        private readonly ICollaboratorRL collaboratorRL;
        private readonly IEmailService emailService;

        public CollaboratorBL(
            ICollaboratorRL collaboratorRL,
            IEmailService emailService)
        {
            this.collaboratorRL = collaboratorRL;
            this.emailService = emailService;
        }

        public async Task<List<CollaboratorResponseDTO>>GetCollaborators(int noteId, int ownerId)
        {
            return await collaboratorRL.GetCollaborators(noteId,ownerId);
        }

        public async Task<CollaboratorResponseDTO> AddCollaborator(
            int noteId,
            string email,
            int ownerId)
        {
            var result = await collaboratorRL.AddCollaborator(
                noteId,
                email,
                ownerId);

            if (result != null)
            {
                string subject = "Fundoo Notes Collaboration Invitation";

                string body = $@"
                <h2>Fundoo Notes</h2>

                <p>You have been added as a collaborator.</p>

                <p>
                    <b>Note Id:</b> {noteId}
                </p>

                <p>
                    You can now view and update
                </p>

                <br/>

                <p>
                    Regards,<br/>
                    Fundoo Notes Team
                </p>";

                await emailService.SendEmail(
                    email,
                    subject,
                    body);
            }

            return result;
        }

        public async Task<bool> RemoveCollaborator(
            int noteId,
            string email,
            int ownerId)
        {
            return await collaboratorRL.RemoveCollaborator(
                noteId,
                email,
                ownerId);
        }
    }
}