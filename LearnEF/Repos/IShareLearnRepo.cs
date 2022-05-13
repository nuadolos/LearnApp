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
        Task<List<User>> GetUsersAsync(int learnId);
        Task<List<Learn>> GetLearnsAsync(string userId);
    }
}
