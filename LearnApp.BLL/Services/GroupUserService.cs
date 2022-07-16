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

        public async Task JoinGroupByInviteCodeAsync(Guid inviteGuid, Guid userGuid)
        {
            var group = await _repo.GetGroupByInviteCodeAsync(inviteGuid);

            if (group == null)
                throw new Exception($"Группа с пригласительным кодом {inviteGuid} не существует");

            if (group.GroupUsers.Any(gu => gu.UserGuid == userGuid) || group.UserGuid == userGuid)
                throw new Exception($"Пользователь {userGuid} уже имеет доступ к группе {group.Guid}");

            var groupUser = new GroupUser {
                GroupGuid = group.Guid,
                UserGuid = userGuid,                           // todo: протестировать,
                GroupRole = new GroupRole { Code = "Студент" } // сможет ли записаться guid в таком формате
            };

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

        public async Task JoinGroupByGuidAsync(Guid groupGuid, Guid userGuid)
        {
            var group = await _repo.GetGroupByGuidAsync(groupGuid);

            if (group == null)
                throw new Exception($"Группа {groupGuid} не существует");

            if (group.GroupUsers.Any(gu => gu.UserGuid == userGuid) || group.UserGuid == userGuid)
                throw new Exception($"Пользователь {userGuid} уже имеет доступ к группе {group.Guid}");

            // todo: протестировать, сможет ли записаться guid в таком формате
            var groupUser = new GroupUser
            {
                GroupGuid = group.Guid,
                UserGuid = userGuid,
                GroupRole = group.GroupType.Code == "Общий" 
                    ? new GroupRole { Code = "Студент" } 
                    : new GroupRole { Code = "Общий" }
            };

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
