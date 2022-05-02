using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IShareLearn : IRepo<ShareLearn>
    {
        bool ContainedInLearn(string userId);
        bool ContainedInUser(int learnId);
    }
}
