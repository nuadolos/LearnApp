using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Services
{
    public class ShareNoteService
    {
        private readonly IShareNoteRepo _repo;

        public ShareNoteService(IShareNoteRepo repo) => 
            _repo = repo;

        /// <summary>
        /// Возвращает список пользователей,
        /// кому открыт доступ к конкретной заметке
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <returns></returns>
        public async Task<List<User>> GetShareNoteUsersAsync(Guid noteGuid) =>
            await _repo.GetUsersAsync(noteGuid);

        /// <summary>
        /// Возвращает список заметок, 
        /// к которым имеет доступ конкретный пользователь
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<List<Note>> GetShareAccessNotes(Guid userGuid) =>
            await _repo.GetNotesAsync(userGuid);

        /// <summary>
        /// Разрешает конкретному пользователю доступ 
        /// на просмотр конкретной заметки
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AddShareAccessAsync(Guid noteGuid, Guid userGuid)
        {
            ShareNote shareNote = new() {
                NoteGuid = noteGuid,
                UserGuid = userGuid
            };

            try
            {
                await _repo.AddAsync(shareNote);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При открытии доступа к заметке пользователю {userGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Убирает у конкретного пользователя доступ
        /// на просмотр конкретной заметки
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RemoveShareAccessAsync(Guid noteGuid, Guid userGuid)
        {
            var shareNote = await _repo.GetShareNoteAsync(noteGuid, userGuid);

            if (shareNote == null)
                throw new Exception($"В базе данных записи о ShareNote(noteGuid: {noteGuid}, userGuid: {userGuid}) не существует");

            try
            {
                await _repo.DeleteAsync(shareNote);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При блокировки доступа к заметке пользователю {userGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}
