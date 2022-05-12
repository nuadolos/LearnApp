using LearnEF.Context;
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
    public class ShareLearnRepo : BaseRepo<ShareLearn>, IShareLearnRepo
    {
        public ShareLearnRepo() : base()
        { }

        public ShareLearnRepo(LearnContext context) : base(context)
        { }

        public List<Learn> GetLearns(string userId)
        {
            List<Learn> userLearns = new List<Learn>();

            Context.ShareLearn
                .Include(sl => sl.Learn)
                .Where(sl => sl.UserId == userId)
                .ForEachAsync(sl => userLearns.Add(sl.Learn))
                .Wait();

            return userLearns;
        }

        public List<User> GetUsers(int learnId)
        {
            List<User> learnUsers
                = new List<User>();

            Context.ShareLearn
                .Include(sl => sl.User)
                .Where(sl => sl.LearnId == learnId)
                .ForEachAsync(sl => learnUsers.Add(sl.User))
                .Wait();

            return learnUsers;
        }
    }
}
