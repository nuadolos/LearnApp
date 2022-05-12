using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Repos
{
    public class GroupUserRepo : BaseRepo<GroupUser>, IGroupUserRepo
    {
        public List<User> GetGroupUsers(int groupId)
        {
            List<User> groupUsers = new List<User>();

            Context.GroupUser
                .Include(gu => gu.User)
                .Where(gu => gu.GroupId == groupId)
                .ForEachAsync(gu => groupUsers.Add(gu.User))
                .Wait();

            return groupUsers;
        }
    }
}
