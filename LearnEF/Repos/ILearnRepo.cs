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
    public interface ILearnRepo : IRepo<Learn>
    {
        bool GroupIsNull(int groupId);
        bool CanChangeLearn(int learnId, string userId);
        bool CanChangeLearn(int learnId, int groupId, string userId);
        bool IsAuthor(int learnId, string userId);
        bool IsCreater(int groupId, string userId);
        bool IsMemberGroup(int learnId, int groupId, string userId);
        bool SharedWith(int learnId, string userId);
        List<Learn> UserLearns(string userId);
        List<Learn> GroupLearns(int groupId);
        List<User> SharedUsers(int learnId);
        List<SourceLore> GetSourceLores();
    }
}
