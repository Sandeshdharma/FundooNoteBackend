using BusinessLayer.IBusiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs.NoteDTOs.RequestDTO;
using Model.DTOs.NoteDTOs.ResponseDTO;
using RepositoryLayer.Redis;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly IFundooBL fundooBL;
        private readonly ILogger<NotesController> _logger;
        private readonly IRedisService _redisService;

        public NotesController(
            IFundooBL fundooBL,
            ILogger<NotesController> logger,
            IRedisService redisService)
        {
            this.fundooBL = fundooBL;
            this._logger = logger;
            this._redisService = redisService;
        }

        [HttpPost("CreateNote")]
        public async Task<IActionResult> CreateNote(NoteRequestDTO note)
        {
            try
            {
                var userId = GetUserIdFromClaims();

                _logger.LogInformation(
                    "Create Note Request Received For UserId : {UserId}",
                    userId);

                var result = await fundooBL.CreateNote(note, userId);

                await _redisService.RemoveData($"Notes_{userId}");

                _logger.LogInformation(
                    "Note Created Successfully For UserId : {UserId}",
                    userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error Occurred While Creating Note");

                throw;
            }
        }

        [HttpGet("GetAllActiveNotes")]
        public async Task<IActionResult> GetAllActiveNotes()
        {
            var userId = GetUserIdFromClaims();
            var cacheKey = $"Notes_{userId}";

            var cachedNotes = await _redisService.GetData<List<NoteResponseDTO>>(cacheKey);
            if (cachedNotes != null)
            {
                return Ok(cachedNotes);
            }

            var result = await fundooBL.GetAllActiveNotes(userId);

            if (result != null)
            {
                await _redisService.SetData(cacheKey, result, TimeSpan.FromMinutes(15));
            }

            return Ok(result);
        }

        [HttpGet("GetArchivedNotes")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.GetArchivedNotes(userId);
            return Ok(result);
        }

        [HttpGet("GetTrashedNotes")]
        public async Task<IActionResult> GetTrashedNotes()
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.GetTrashedNotes(userId);
            return Ok(result);
        }

        [HttpGet("SearchNotes")]
        public async Task<IActionResult> SearchNotes(string keyword)
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.SearchNotes(keyword, userId);
            return Ok(result);
        }

        [HttpPut("UpdateNote")]
        public async Task<IActionResult> UpdateNote(int noteId, NoteRequestDTO note)
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.UpdateNote(noteId, note, userId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Note not found or user unauthorized."
                });
            }

            await _redisService.RemoveData($"Notes_{userId}");

            return Ok(result);
        }

        [HttpDelete("DeleteForever")]
        public async Task<IActionResult> DeleteForever(int noteId)
        {
            var userId = GetUserIdFromClaims();

            var result = await fundooBL.DeleteForever(noteId, userId);
            
            await _redisService.RemoveData($"Notes_{userId}");

            return Ok(result);
        }

        [HttpPatch("TogglePin")]
        public async Task<IActionResult> TogglePin(int noteId)
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.TogglePin(noteId, userId);
            await _redisService.RemoveData($"Notes_{userId}");
            return Ok(result);
        }

        [HttpPatch("ToggleArchive")]
        public async Task<IActionResult> ToggleArchive(int noteId)
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.ToggleArchive(noteId, userId);
            await _redisService.RemoveData($"Notes_{userId}");
            return Ok(result);
        }

        [HttpPatch("ToggleTrash")]
        public async Task<IActionResult> ToggleTrash(int noteId)
        {
            var userId = GetUserIdFromClaims();
            var result = await fundooBL.ToggleTrash(noteId, userId);
            await _redisService.RemoveData($"Notes_{userId}");
            return Ok(result);
        }

        private int GetUserIdFromClaims()
        {
            var claim = User.FindFirst("UserId") ??
                        User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new UnauthorizedAccessException(
                    "User identification token claim is missing.");
            }

            return Convert.ToInt32(claim.Value);
        }
    }
}