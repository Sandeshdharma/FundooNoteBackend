using BusinessLayer.IBusiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.LabelDTOs.RequestDTOs;
using RepositoryLayer.Redis;
using System;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly IRedisService _redisService;

        public LabelController(ILabelBL labelBL, IRedisService redisService)
        {
            this.labelBL = labelBL;
            this._redisService = redisService;
        }

        [HttpPost("CreateLabel")]
        public async Task<IActionResult> CreateLabel(
            CreateLabelDTO request)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await labelBL.CreateLabel(
                request,
                userId);

            if (result == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Label already exists"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Label created successfully",
                Data = result
            });
        }

        [HttpGet("GetAllLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await labelBL.GetAllLabels(userId);

            return Ok(new
            {
                Success = true,
                Message = "Labels fetched successfully",
                Data = result
            });
        }
        [HttpPut("UpdateLabel/{labelId}")]
        public async Task<IActionResult> UpdateLabel(int labelId, [FromBody] UpdateLabelDTO request)
        {
            int userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var result = await labelBL.UpdateLabel(labelId, request.LabelName, userId);

            if (result == null)
            {
                return NotFound(new { Success = false, Message = "Label not found or you do not have access" });
            }

            return Ok(new { Success = true, Message = "Label updated successfully", Data = result });
        }

        [HttpDelete("DeleteLabel/{labelId}")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            int userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var result = await labelBL.DeleteLabel(labelId, userId);

            if (!result)
            {
                return NotFound(new { Success = false, Message = "Label not found or you do not have access" });
            }

            return Ok(new { Success = true, Message = "Label deleted successfully" });
        }

        [HttpPost("AddLabelToNote")]
        public async Task<IActionResult> AddLabelToNote(int noteId, int labelId)
        {
            int userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var result = await labelBL.AddLabelToNote(noteId, labelId, userId);

            if (!result)
            {
                return BadRequest(new { Success = false, Message = "Failed to add label to note. It may already exist or note/label not found." });
            }

            await _redisService.RemoveData($"Notes_{userId}");

            return Ok(new { Success = true, Message = "Label added to note successfully" });
        }

        [HttpDelete("RemoveLabelFromNote")]
        public async Task<IActionResult> RemoveLabelFromNote(int noteId, int labelId)
        {
            int userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var result = await labelBL.RemoveLabelFromNote(noteId, labelId, userId);

            if (!result)
            {
                return BadRequest(new { Success = false, Message = "Failed to remove label from note. It may not exist." });
            }

            await _redisService.RemoveData($"Notes_{userId}");

            return Ok(new { Success = true, Message = "Label removed from note successfully" });
        }
    }
}

