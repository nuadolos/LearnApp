using LearnEF.Context;
using LearnEF.Entities;
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

        public List<Learn> GetRelatedData() =>
            Context.Learn.FromSqlRaw("SELECT * FROM Learn").Include(lore => lore.SourceLore).ToList();

        public List<SourceLore> GetSourceLores() =>
            Context.SourceLore.ToList();

        public List<Learn> Search(string searchString) =>
            Context.Learn.Where(l => EF.Functions.Like(l.Title, $"%{searchString}%")).ToList();

        public List<Learn> UserLearns(string userId) =>
            Context.Learn.Where(u => u.UserId == userId).ToList();

        public List<ShareLearn> SharedUsers(int learnId) =>
            Context.ShareLearn.Where(u => u.LearnId == learnId).ToList();

        public bool IsAuthor(int learnId, string userId) =>
            Context.Learn.FirstOrDefault(u => u.UserId == userId && u.Id == learnId) != null ? true : false;

        public bool SharedWith(int learnId, string userId) =>
            Context.ShareLearn.FirstOrDefault(u => u.UserId == userId && u.LearnId == learnId) != null ? true : false;

        public bool CanChangeLearn(int learnId, string userId)
        {
            var shareLearn = Context.ShareLearn.FirstOrDefault(u => u.UserId == userId && u.LearnId == learnId);

            if (shareLearn != null)
            {
                if (shareLearn.CanChange)
                    return true;
            }

            return false;
        }
    }
}
