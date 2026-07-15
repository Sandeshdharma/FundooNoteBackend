using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IBusiness;
using Microsoft.AspNetCore.Authorization;


namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollaboratorController : ControllerBase
    {

        private readonly ICollaboratorBL collaboratorBL;

        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            this.collaboratorBL = collaboratorBL;
        }


        [HttpGet("GetCollaborators")]
        public async Task<IActionResult> GetCollaborators(int noteId)
        {
            int ownerId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await collaboratorBL.GetCollaborators(
                noteId,
                ownerId);

            if (result == null)
            {
                return BadRequest(
                    new
                    {
                        Success = false,
                        Message = "Only owner can view collaborators."
                    });
            }

            return Ok(result);
        }






        [HttpPost("AddCollaborator")]

        public async Task<IActionResult> AddCollaborator(int noteId, string email)
        {
            int ownerId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var result = await collaboratorBL.AddCollaborator(noteId, email, ownerId);
            if (result == null)
            {
                return BadRequest("Collaborator Cant be added");

            }
            return Ok(result);
        }
        [HttpDelete("RemoveCollaborator")]
        [Authorize]
        public async Task<IActionResult>RemoveCollaborator(int noteId,string email)
        {
            int ownerId = Convert.ToInt32(User.FindFirst("UserId")?.Value);

            var result = await collaboratorBL.RemoveCollaborator(noteId, email, ownerId);
                   

            if (!result)
            {
                return BadRequest(
                    new
                    {
                        Success = false,
                        Message = "Collaborator Not Found Or You Are Not Owner"
                    });
            }

            return Ok(
                new
                {
                    Success = true,
                    Message = "Collaborator Removed Successfully"
                });
        }
    }
}
