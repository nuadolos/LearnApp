﻿using LearnApp.BL.Models;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BL.Services
{
    public class NoteService
    {
        private readonly INoteRepo _repo;

        public NoteService(INoteRepo repo) =>
            _repo = repo;


        /// <summary>
        /// Возвращает список заметок конкретного пользователя
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <returns></returns>
        public async Task<List<Note>> GetUserNotesAsync(Guid noteGuid) =>
            await _repo.GetUserNotesAsync(noteGuid);

        /// <summary>
        /// Создает заметку конкретного пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Note> CreateNoteAsync(RequestNoteModel model)
        {
            Note note = new() {
                Title = model.Title,
                Description = model.Description,
                Link = model.Link,
                IsVisible = model.IsVisible,
                NoteTypeGuid = model.NoteTypeGuid,
                UserGuid = model.UserGuid
            };

            try
            {
                await _repo.AddAsync(note);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При добавление заметки у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }

            return note;
        }

        /// <summary>
        /// Обновляет свойства заметки, которые изменил пользователь
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateNoteAsync(Guid noteGuid, RequestNoteModel model)
        {
            var note = await _repo.GetRecordAsync(noteGuid);

            if (note == null)
                throw new Exception($"Заметки {noteGuid} не существует");

            if (note.UserGuid != model.UserGuid)
                throw new Exception($"Пользователь {model.UserGuid} не является создателем заметки {note.Guid}");

            note.Title = model.Title;
            note.Description = model.Description;
            note.Link = model.Link;
            note.IsVisible = model.IsVisible;
            note.NoteTypeGuid = model.NoteTypeGuid;

            try
            {
                await _repo.UpdateAsync(note);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При сохранении заметки у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Удаляет заметку пользователя, создавший ее
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RemoveNoteAsync(RequestRemoveDataModel model)
        {
            var note = await _repo.GetRecordAsync(model.Guid);

            if (note == null)
                throw new Exception($"Заметки {model.Guid} не существует");

            if (note.UserGuid != model.UserGuid)
                throw new Exception($"Пользователь {model.UserGuid} не является создателем заметки {note.Guid}");

            try
            {
                await _repo.DeleteAsync(model.Guid, model.Timestamp);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При удалении заметки у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}