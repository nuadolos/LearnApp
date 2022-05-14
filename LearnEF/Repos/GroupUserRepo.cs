﻿using LearnEF.Entities;
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

        public async Task<List<User>> GetGroupUsers(int groupId)
        {
            List<User> groupUsers = new List<User>();

            await Context.GroupUser
                .Include(gu => gu.User)
                .Where(gu => gu.GroupId == groupId)
                .ForEachAsync(gu => groupUsers.Add(gu.User));

            return groupUsers;
        }

        public async Task<string> AcceptedInviteAsync(string inviteId, string userId)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.CodeInvite == inviteId);

            if (group == null)
                return "Код доступа не действителен";

            var member = await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupId == group.Id && gu.UserId == userId);

            if (group.UserId == userId || member != null)
                return "Вы уже участник группы";

            GroupUser groupUser = new GroupUser
            {
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

            await Context.GroupUser.AddAsync(groupUser);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch(DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
