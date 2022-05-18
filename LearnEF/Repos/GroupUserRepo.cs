using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnEF.Context;
using LearnEF.Entities.ErrorModel;

namespace LearnEF.Repos
{
    public class GroupUserRepo : BaseRepo<GroupUser>, IGroupUserRepo
    {
        public GroupUserRepo() : base()
        { }

        public GroupUserRepo(LearnContext context) : base(context)
        { }

        public List<User> GetGroupUsers(int groupId)
        {
            List<User> groupUsers = new List<User>();

            var gUsers = Context.GroupUser
                .Include(gu => gu.User)
                .Include(gu => gu.GroupRole)
                .Where(gu => gu.GroupId == groupId);

            foreach (var item in gUsers.AsParallel())
            {
                item.User.GroupRoleId = item.GroupRoleId;
                item.User.GroupId = item.GroupId;
                groupUsers.Add(item.User);
            }

            return groupUsers;
        }

        public async Task<string> JoinOpenGroupAsync(int groupId, string userId)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                return "Группы не существует";

            GroupUser groupUser = new GroupUser {
                GroupId = group.Id,
                UserId = userId,
                GroupRoleId = group.GroupTypeId switch
                {
                    // Если тип группы - равноправный,
                    // то роль у пользователя - общий
                    1 => 3,

                    // Если тип группы - класс,
                    // то роль у пользователя - студент
                    _ => 1
                }
            };

            try
            {
                await AddAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }
            
            return string.Empty;
        }

        public async Task<string> AcceptedInviteAsync(string inviteId, string userId)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.CodeInvite == inviteId || g.CodeAdmin == inviteId);

            if (group == null)
                return "Код доступа не существует";

            var member = await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupId == group.Id && gu.UserId == userId);

            if (group.UserId == userId || member != null)
                return "Вы уже участник группы";

            GroupUser groupUser = new GroupUser
            {
                GroupId = group.Id,
                UserId = userId,
            };

            if (group.CodeInvite == inviteId)
                groupUser.GroupRoleId = 1;
            else
                groupUser.GroupRoleId = 2;

            try
            {
                await AddAsync(groupUser);
            }
            catch(DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> KickUserAsync(int groupId, string userId)
        {
            var groupUser = await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupId == groupId && gu.UserId == userId);

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
