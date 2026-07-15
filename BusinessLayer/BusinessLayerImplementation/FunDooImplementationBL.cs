using BusinessLayer.IBusiness;
using Microsoft.Extensions.Logging;
using Model.DTOs.NoteDTOs.RequestDTO;
using Model.DTOs.NoteDTOs.ResponseDTO;
using Model.Entity;
using RepositoryLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Linq; // Added for Select and ToList
using System.Threading.Tasks;

namespace BusinessLayer.BusinessLayerImplementation
{
    public class FunDooImplementationBL : IFundooBL
    {
        private readonly IFundooRL repository;
        private ILogger<FunDooImplementationBL> logger;
        public FunDooImplementationBL(IFundooRL repository)
        {
            this.repository = repository;
        }

        public async Task<NoteResponseDTO> CreateNote(NoteRequestDTO note, int userId)
        {
            NotesModel notesModel = new NotesModel()
            {
                Title = note.Title,
                Description = note.Description,
                Remainder = note.Reminder,
                BackgroundColor = note.BackgroundColor,
                Image = note.Image,
                UserId = userId
            };

            var result = await repository.CreateNotes(notesModel, userId);

            //logger.LogInformation("Notes Created Sucessfully");


            return new NoteResponseDTO()
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Reminder = result.Remainder,
                BackgroundColor = result.BackgroundColor,
                Image = result.Image,
                Pin = result.Pin,
                Archive = result.Archive,
                Trash = result.Trash,
                Created = result.Created,
                Edited = result.Edited,
                CreatorId = result.UserId,
                CreatorFirstName = result.UserModel?.FirstName,
                CreatorLastName = result.UserModel?.LastName,
                CreatorEmail = result.UserModel?.Email
            };
        }

        public async Task<List<NoteResponseDTO>> GetAllActiveNotes(int userId)
        {
            var notes = await repository.GetAllActiveNotes(userId);
            

            return notes.Select(x => new NoteResponseDTO()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Reminder = x.Remainder,
                BackgroundColor = x.BackgroundColor,
                Image = x.Image,
                Pin = x.Pin,
                Archive = x.Archive,
                Trash = x.Trash,
                Created = x.Created,
                Edited = x.Edited,
                CreatorId = x.UserId,
                CreatorFirstName = x.UserModel?.FirstName,
                CreatorLastName = x.UserModel?.LastName,
                CreatorEmail = x.UserModel?.Email,
                Labels = x.NoteLabels?.Select(nl => new Model.DTOs.LabelDTOs.ResponseDTOs.LabelResponseDTO { LabelId = nl.Label.LabelId, LabelName = nl.Label.LabelName, UserId = nl.Label.UserId }).ToList()
            }).ToList();
        }

        public async Task<List<NoteResponseDTO>> GetArchivedNotes(int userId)
        {
            var notes = await repository.GetArchivedNotes(userId);

            return notes.Select(x => new NoteResponseDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Reminder = x.Remainder,
                BackgroundColor = x.BackgroundColor,
                Image = x.Image,
                Pin = x.Pin,
                Archive = x.Archive,
                Trash = x.Trash,
                Created = x.Created,
                Edited = x.Edited,
                CreatorId = x.UserId,
                CreatorFirstName = x.UserModel?.FirstName,
                CreatorLastName = x.UserModel?.LastName,
                CreatorEmail = x.UserModel?.Email,
                Labels = x.NoteLabels?.Select(nl => new Model.DTOs.LabelDTOs.ResponseDTOs.LabelResponseDTO { LabelId = nl.Label.LabelId, LabelName = nl.Label.LabelName, UserId = nl.Label.UserId }).ToList()
            }).ToList();
        }

        public async Task<List<NoteResponseDTO>> GetTrashedNotes(int userId)
        {
            var notes = await repository.GetTrashedNotes(userId);

            return notes.Select(x => new NoteResponseDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Reminder = x.Remainder,
                BackgroundColor = x.BackgroundColor,
                Image = x.Image,
                Pin = x.Pin,
                Archive = x.Archive,
                Trash = x.Trash,
                Created = x.Created,
                Edited = x.Edited,
                CreatorId = x.UserId,
                CreatorFirstName = x.UserModel?.FirstName,
                CreatorLastName = x.UserModel?.LastName,
                CreatorEmail = x.UserModel?.Email,
                Labels = x.NoteLabels?.Select(nl => new Model.DTOs.LabelDTOs.ResponseDTOs.LabelResponseDTO { LabelId = nl.Label.LabelId, LabelName = nl.Label.LabelName, UserId = nl.Label.UserId }).ToList()
            }).ToList();
        }

        public async Task<List<NoteResponseDTO>> SearchNotes(string keyword, int userId)
        {
            var notes = await repository.SearchNotes(keyword, userId);

            return notes.Select(x => new NoteResponseDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Reminder = x.Remainder,
                BackgroundColor = x.BackgroundColor,
                Image = x.Image,
                Pin = x.Pin,
                Archive = x.Archive,
                Trash = x.Trash,
                Created = x.Created,
                Edited = x.Edited,
                CreatorId = x.UserId,
                CreatorFirstName = x.UserModel?.FirstName,
                CreatorLastName = x.UserModel?.LastName,
                CreatorEmail = x.UserModel?.Email,
                Labels = x.NoteLabels?.Select(nl => new Model.DTOs.LabelDTOs.ResponseDTOs.LabelResponseDTO { LabelId = nl.Label.LabelId, LabelName = nl.Label.LabelName, UserId = nl.Label.UserId }).ToList()
            }).ToList();
        }

        public async Task<NoteResponseDTO> UpdateNote(int noteId, NoteRequestDTO note, int userId)
        {
            NotesModel notesModel = new NotesModel()
            {
                Title = note.Title,
                Description = note.Description,
                Remainder = note.Reminder,
                BackgroundColor = note.BackgroundColor,
                Image = note.Image
            };

            var result = await repository.UpdateNote(noteId, notesModel, userId);

            if (result == null)
            {
                return null;
            }

            return new NoteResponseDTO()
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Reminder = result.Remainder,
                BackgroundColor = result.BackgroundColor,
                Image = result.Image,
                Pin = result.Pin,
                Archive = result.Archive,
                Trash = result.Trash,
                Created = result.Created,
                Edited = result.Edited
            };
        }

        public async Task<bool> DeleteForever(int noteId, int userId)
        {
            return await repository.DeleteForever(noteId, userId);
        }

        public async Task<bool> TogglePin(int noteId, int userId)
        {
            return await repository.TogglePin(noteId, userId);
        }

        public async Task<bool> ToggleArchive(int noteId, int userId)
        {
            return await repository.ToggleArchive(noteId, userId);
        }

        public async Task<bool> ToggleTrash(int noteId, int userId)
        {
            return await repository.ToggleTrash(noteId, userId);
        }
    }
}