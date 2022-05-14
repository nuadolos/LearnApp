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

        public async Task<List<SourceLore>> GetSourceLoresAsync() =>
            await Context.SourceLore.ToListAsync();

        public async Task<List<Learn>> UserLearnsAsync(string userId) =>
            await Context.Learn.Where(u => u.UserId == userId).ToListAsync();

        public async Task<List<User>> SharedUsersAsync(int learnId)
        {
            List<User> users = new List<User>();

            await Context.ShareLearn
                .Include(u => u.User)
                .Where(u => u.LearnId == learnId)
                .ForEachAsync(u => users.Add(u.User));

            return users;
        }

        public async Task<List<Learn>> GroupLearnsAsync(int groupId)
        {
            List<Learn> learns = new List<Learn>();

            await Context.GroupLearn
                .Include(l => l.Learn)
                .Where(l => l.GroupId == groupId)
                .ForEachAsync(l => learns.Add(l.Learn));

            return learns;
        }

        public async Task<bool> GroupIsNullAsync(int groupId) =>
            await Context.Group.FirstAsync(g => g.Id == groupId) == null;

        public async Task<bool> IsMemberGroupAsync(int learnId, int groupId, string userId)
        {
            var groupLearn = await Context.GroupLearn.FirstOrDefaultAsync(
                gu => gu.GroupId == groupId && gu.LearnId == learnId);

            if (groupLearn != null)
            {
                var groupUser = await Context.GroupUser.FirstOrDefaultAsync(
                    gu => gu.GroupId == groupId && gu.UserId == userId);

                if (groupUser != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsAuthorAsync(int learnId, string userId) =>
            await Context.Learn.FirstOrDefaultAsync(u => u.UserId == userId && u.Id == learnId) != null;

        public async Task<bool> SharedWithAsync(int learnId, string userId) =>
            await Context.ShareLearn.FirstOrDefaultAsync(u => u.UserId == userId && u.LearnId == learnId) != null;

        public async Task<bool> CanChangeLearnAsync(int learnId, string userId)
        {
            var shareLearn = await Context.ShareLearn.FirstOrDefaultAsync(u => u.UserId == userId && u.LearnId == learnId);

            if (shareLearn?.CanChange == true)
                return true;

            return false;
        }

        public async Task<bool> CanChangeLearnAsync(int learnId, int groupId, string userId)
        {
            var groupLearn = await Context.GroupLearn.FirstOrDefaultAsync(
                gu => gu.GroupId == groupId && gu.LearnId == learnId);

            if (groupLearn != null)
            {
                var groupUser = await Context.GroupUser.FirstOrDefaultAsync(
                    gu => gu.GroupId == groupId && gu.UserId == userId && gu.GroupRoleId != 1);

                if (groupUser != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsCreaterAsync(int groupId, string userId) =>
            await Context.Group.FirstOrDefaultAsync(u => u.Id == groupId && u.UserId == userId) != null;
    }
}
