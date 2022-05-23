using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public class LearnRepo : BaseRepo<Learn>, ILearnRepo
    {
        public LearnRepo() : base()
        { }

        public LearnRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Learn>> GetGroupLearnsAsync(int groupId) =>
            await Context.Learn.Where(l => l.GroupId == groupId).ToListAsync();

        public async Task<Group?> GetGroupAsync(int groupId) =>
            await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId);

        public async Task<Learn?> GetLearnAsync(int learnId) =>
            await Context.Learn.FirstOrDefaultAsync(l => l.Id == learnId);

        public async Task<bool> IsMemberGroupAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupId == groupId && gu.UserId == userId) != null;

        public async Task<bool> CanChangeLearnAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                    gu => gu.GroupId == groupId && gu.UserId == userId && gu.GroupRoleId != 1) != null;
    }
}
