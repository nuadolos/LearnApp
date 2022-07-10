using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.DAL.Exceptions;

namespace LearnApp.DAL.Repos
{
    public class GroupUserRepo : BaseRepo<GroupUser>, IGroupUserRepo
    {
        public GroupUserRepo() : base()
        { }

        public GroupUserRepo(LearnContext context) : base(context)
        { }

        public async Task<List<User>> GetGroupUsersAsync(Guid groupGuid)
        {
            List<User> groupUser = new List<User>();

            await Context.GroupUser
                .Include(gu => gu.User)
                .Where(gu => gu.GroupGuid == groupGuid)
                .ForEachAsync(gu => groupUser.Add(gu.User));

            return groupUser;
        }

        public async Task<string> UserRoleInGroupAsync(Guid groupGuid, Guid userGuid)
        {
            // РЕАЛИЗОВАТЬ ИНАЧЕ
            var groupUser = await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid);

            if (groupUser == null)
            {
                var creator = await Context.Group.FirstOrDefaultAsync(
                    g => g.Guid == groupGuid && g.UserGuid == userGuid);

                if (creator != null)
                    return "Создатель";
                else
                    return string.Empty;
            }

            return string.Empty;

            //return groupUser.Guid switch
            //{
            //    1 => "Студент",
            //    2 => "Преподаватель",
            //    _ => "Общий"
            //};
        }

        public async Task<string> JoinOpenGroupAsync(Guid groupGuid, Guid userGuid)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.Guid == groupGuid);

            if (group == null)
                return "Группы не существует";

            var member = await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupGuid == group.Guid && gu.UserGuid == userGuid);

            if (group.UserGuid == userGuid || member != null)
                return "Вы уже участник группы";

            member = new GroupUser
            {
                GroupGuid = group.Guid,
                UserGuid = userGuid,
                //GroupRoleId = group.GroupTypeId switch
                //{
                //    // Если тип группы - равноправный,
                //    // то роль у пользователя - общий
                //    1 => 3,

                //    // Если тип группы - класс,
                //    // то роль у пользователя - студент
                //    _ => 1
                //}
            };

            try
            {
                await AddAsync(member);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> AcceptedInviteAsync(Guid inviteGuid, Guid userGuid)
        {
            var group = await Context.Group.FirstOrDefaultAsync(
                g => g.InviteCode == inviteGuid || g.AdminCode == inviteGuid);

            if (group == null)
                return "Код доступа не существует";

            var member = await Context.GroupUser.Include(gu => gu.GroupRole)
                .FirstOrDefaultAsync(gu => gu.GroupGuid == group.Guid && gu.UserGuid == userGuid);

            if (group.UserGuid == userGuid || member != null)
                return "Вы уже участник группы";

            member = new GroupUser
            {
                GroupGuid = group.GroupTypeGuid,
                UserGuid = userGuid
            };

            //if (group.InviteCode == inviteGuid)
            //    member.GroupRole = new GroupRole();
            //else
            //    member.GroupRoleId = 2;

            try
            {
                await AddAsync(member);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> KickUserAsync(Guid groupGuid, Guid userGuid)
        {
            var groupUser = await Context.GroupUser.FirstOrDefaultAsync(
               gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid);

            if (groupUser == null)
                return "Такого пользователя в группе не существует";

            try
            {
                await DeleteAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
