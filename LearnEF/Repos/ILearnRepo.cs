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
        Task<List<Learn>> GetGroupLearnsAsync(int groupId);
        Task<Group?> GetGroupAsync(int groupId);
        Task<Learn?> GetLearnAsync(int learnId);
        Task<bool> IsMemberGroupAsync(int groupId, string userId);
        Task<bool> CanChangeLearnAsync(int groupId, string userId);
    }
}
