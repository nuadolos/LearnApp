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

        public List<SourceLore> GetSourceLores() =>
            Context.SourceLore.ToList();

        public List<Learn> UserLearns(string userId) =>
            Context.Learn.Where(u => u.UserId == userId).ToList();

        public List<User> SharedUsers(int learnId)
        {
            List<User> users = new List<User>();

            Context.ShareLearn
                .Include(u => u.User)
                .Where(u => u.LearnId == learnId)
                .ForEachAsync(u => users.Add(u.User))
                .Wait();

            return users;
        }

        public List<Learn> GroupLearns(int groupId)
        {
            List<Learn> learns = new List<Learn>();

            Context.GroupLearn
                .Include(l => l.Learn)
                .Where(l => l.GroupId == groupId)
                .ForEachAsync(l => learns.Add(l.Learn))
                .Wait();

            return learns;
        }

        public bool GroupIsNull(int groupId) =>
            Context.Group.FirstOrDefault(g => g.Id == groupId) == null ? true : false;

        public bool IsMemberGroup(int learnId, int groupId, string userId)
        {
            var groupLearn = Context.GroupLearn.FirstOrDefault(
                gu => gu.GroupId == groupId && gu.LearnId == learnId);

            if (groupLearn != null)
            {
                var groupUser = Context.GroupUser.FirstOrDefault(
                gu => gu.GroupId == groupId && gu.UserId == userId);

                if (groupUser != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAuthor(int learnId, string userId) =>
            Context.Learn.FirstOrDefault(u => u.UserId == userId && u.Id == learnId) != null ? true : false;

        public bool SharedWith(int learnId, string userId) =>
            Context.ShareLearn.FirstOrDefault(u => u.UserId == userId && u.LearnId == learnId) != null ? true : false;

        public bool CanChangeLearn(int learnId, string userId)
        {
            var shareLearn = Context.ShareLearn.FirstOrDefault(u => u.UserId == userId && u.LearnId == learnId);

            if (shareLearn?.CanChange == true)
                return true;

            return false;
        }

        public bool CanChangeLearn(int learnId, int groupId, string userId)
        {
            var groupLearn = Context.GroupLearn.FirstOrDefault(
                gu => gu.GroupId == groupId && gu.LearnId == learnId);

            if (groupLearn != null)
            {
                var groupUser = Context.GroupUser.FirstOrDefault(
                    gu => gu.GroupId == groupId && gu.UserId == userId && gu.GroupRoleId != 1);

                if (groupUser != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCreater(int groupId, string userId) =>
            Context.Group.FirstOrDefault(u => u.Id == groupId && u.UserId == userId) != null ? true : false;
    }
}
