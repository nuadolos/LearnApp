using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnEF.Context;

namespace LearnEF.Repos
{
    public class GroupRepo : BaseRepo<Group>, IGroupRepo
    {
        public GroupRepo() : base()
        { }

        public GroupRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Group>> GetUserGroupsAsync(string userId) =>
             await Context.Group.Where(g => g.UserId == userId).ToListAsync();

        public async Task<List<Group>> GetMemberGroupsAsync(string userId)
        {
            List<Group> groups = new List<Group>();

            await Context.GroupUser
                .Include(gu => gu.Group)
                .Where(gu => gu.UserId == userId)
                .ForEachAsync(gu => groups.Add(gu.Group));

            return groups;
        }

        public async Task<List<Group>> GetVisibleGroupsAsync() =>
            await Context.Group.Where(g => g.IsVisible == true).ToListAsync();

        public async Task<bool> IsCreatorAsync(int groupId, string userId) =>
            await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == userId) != null;

        public async Task<bool> IsMemberAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupId == groupId && gu.UserId == userId) != null;
    }
}
