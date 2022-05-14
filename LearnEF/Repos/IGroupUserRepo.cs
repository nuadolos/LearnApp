﻿using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IGroupUserRepo : IRepo<GroupUser>
    {
        Task<List<User>> GetGroupUsers(int groupId);
        Task<string> AcceptedInviteAsync(string inviteId, string userId);
    }
}
