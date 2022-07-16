using LearnApp.BLL.Models.Response;
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
    public class GroupUserService
    {
        private readonly IGroupUserRepo _repo;

        public GroupUserService(IGroupUserRepo repo) =>
            _repo = repo;

        /// <summary>
        /// Возвращает список категорий и 
        /// пользователей, относящиеся к ней, конкретной группы
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <returns></returns>
        public async Task<List<ResponseGroupUserModel>> GetGroupUsersAsync(Guid groupGuid)
        {
            List<ResponseGroupUserModel> model = new List<ResponseGroupUserModel>();

            var creator = await _repo.GetGroupCreatorAsync(groupGuid);
            var userViewData = await _repo.GetGroupUsersAsync(groupGuid);

            foreach (var role in userViewData.GroupBy(e => e.GroupRoleName))
            {
                var users = userViewData.Where(u => u.GroupRoleName == role.Key).ToList();
                model.Add(new ResponseGroupUserModel {
                    RoleName = role.Key,
                    UserCount = users.Count,
                    Users = users
                });
            }

            return model;
        }

        /// <summary>
        /// Открывает доступ к конкретной группе,
        /// имея ее пригласительный код
        /// </summary>
        /// <param name="inviteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task JoinGroupByInviteCodeAsync(Guid inviteGuid, Guid userGuid)
        {
            var group = await _repo.GetGroupByInviteCodeAsync(inviteGuid);

            if (group == null)
                throw new Exception($"Группа с пригласительным кодом {inviteGuid} не существует");

            if (group.GroupUsers.Any(gu => gu.UserGuid == userGuid) || group.UserGuid == userGuid)
                throw new Exception($"Пользователь {userGuid} уже имеет доступ к группе {group.Guid}");

            var groupUser = new GroupUser {
                GroupGuid = group.Guid,
                UserGuid = userGuid
            };

            if (group.GroupTypeCode == "CLASS")
                groupUser.GroupRoleCode = group.InviteCode == inviteGuid ? "STUDENT" : "TEACHER";
            else
                groupUser.GroupRoleCode = "GENERAL";

            try
            {
                await _repo.AddAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При вступлении в группу {group.Guid} с помощью " +
                    $"пригласительного кода {inviteGuid} у пользователя {userGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Открывает доступ к конкретной открытой группе
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task JoinGroupByGuidAsync(Guid groupGuid, Guid userGuid)
        {
            var group = await _repo.GetGroupByGuidAsync(groupGuid);

            if (group == null)
                throw new Exception($"Группа {groupGuid} не существует");

            if (group.GroupUsers.Any(gu => gu.UserGuid == userGuid) || group.UserGuid == userGuid)
                throw new Exception($"Пользователь {userGuid} уже имеет доступ к группе {group.Guid}");

            var groupUser = new GroupUser
            {
                GroupGuid = group.Guid,
                UserGuid = userGuid
            };

            if (group.GroupTypeCode == "CLASS")
                groupUser.GroupRoleCode = "STUDENT";
            else
                groupUser.GroupRoleCode = "GENERAL";

            try
            {
                await _repo.AddAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При вступлении в группу {group.Guid} " +
                    $"у пользователя {userGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Убирает доступ к конкретной группе 
        /// после целенаправленного выхода пользователя
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task LeaveGroupAsync(Guid groupGuid, Guid userGuid)
        {
            var groupUser = await _repo.GetGroupUserAsync(groupGuid, userGuid);

            if (groupUser == null)
                throw new Exception($"Пользователь {userGuid} уже покинул группу {groupGuid}");

            try
            {
                await _repo.DeleteAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При выходе из группы {groupUser.Guid} " +
                    $"у пользователя {userGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}
