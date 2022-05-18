using LearnEF.Entities;
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
        List<User> GetGroupUsers(int groupId);
        Task<string> JoinOpenGroupAsync(int groupId, string userId);
        Task<string> AcceptedInviteAsync(string inviteId, string userId);
        Task<string> KickUserAsync(int groupId, string userId);
    }
}
