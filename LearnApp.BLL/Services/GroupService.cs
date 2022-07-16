using LearnApp.BLL.Models.Request;
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
    public class GroupService
    {
        private readonly IGroupRepo _repo;

        public GroupService(IGroupRepo repo) =>
            _repo = repo;

        /// <summary>
        /// Возвращает список открытых групп
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetVisibleGroupsAsync() =>
            await _repo.GetVisibleGroupsAsync();

        /// <summary>
        /// Возвращает список групп конкретного пользователя,
        /// в которых он состоит с разными ролями
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetUserGroupsAsync(Guid userGuid) =>
            await _repo.GetUserGroupsAsync(userGuid);

        /// <summary>
        /// Возвращает конкретную группу пользователю,
        /// который состоит в ней
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Group> GetGroupAsync(Guid groupGuid, Guid userGuid)
        {
            var group = await _repo.GetGroupByGuidAsync(groupGuid, userGuid);

            if (group == null)
                throw new Exception($"Группы {groupGuid} не существует");

            if (group.UserGuid != userGuid && !group.GroupUsers.Any())
                throw new Exception($"Пользователь {userGuid} пытается запросить данные группы, не имея к ней доступ");

            return group;
        }

        /// <summary>
        /// Создает группу конкретного пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Group> CreateGroupAsync(RequestGroupModel model)
        {
            Group group = new() {
                Title = model.Title,
                Description = model.Description,
                IsVisible = model.IsVisible,
                GroupTypeCode = model.GroupTypeCode,
                UserGuid = model.UserGuid
            };

            try
            {
                await _repo.AddAsync(group);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При добавлении группы у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex); 
            }

            return group;
        }

        /// <summary>
        /// Обновляет свойства групп, которые изменил пользователь
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateGroupAsync(Guid groupGuid, RequestGroupModel model)
        {
            var group = await _repo.GetRecordAsync(groupGuid);

            if (group == null)
                throw new Exception($"Группы {groupGuid} не существует");

            if (group.UserGuid != model.UserGuid)
                throw new Exception($"Пользователь {model.UserGuid} не является создатель группы {group.Guid}");

            group.Title = model.Title;
            group.Description = model.Description;
            group.IsVisible = model.IsVisible;

            try
            {
                await _repo.UpdateAsync(group);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При обновлении группы у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Удаляет группу пользователя, создавший ее
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RemoveGroupAsync(RequestRemoveDataModel model)
        {
            var group = await _repo.GetRecordAsync(model.Guid);

            if (group == null)
                throw new Exception($"Группы {model.Guid} не существует");

            if (group.UserGuid != model.UserGuid)
                throw new Exception($"Пользователь {model.UserGuid} не является создатель группы {group.Guid}");

            try
            {
                await _repo.DeleteAsync(group);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При удалении группы у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}
