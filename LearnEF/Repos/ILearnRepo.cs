using LearnEF.Entities;
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
        bool CanChangeLearn(int learnId, string userId);
        bool IsAuthor(int learnId, string userId);
        bool SharedWith(int learnId, string userId);
        List<Learn> UserLearns(string userId);
        List<ShareLearn> SharedUsers(int learnId);
        List<Learn> Search(string searchString);
        List<Learn> GetRelatedData();
        List<SourceLore> GetSourceLores();
    }
}
