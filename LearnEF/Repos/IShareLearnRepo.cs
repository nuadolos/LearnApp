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
    public interface IShareLearnRepo : IRepo<ShareLearn>
    {
        List<User> GetUsers(int learnId);
        List<Learn> GetLearns(string userId);
    }
}
