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
        Task<bool> GroupIsNullAsync(int groupId);
        Task<bool> CanChangeLearnAsync(int learnId, string userId);
        Task<bool> CanChangeLearnAsync(int learnId, int groupId, string userId);
        Task<bool> IsAuthorAsync(int learnId, string userId);
        Task<bool> IsCreaterAsync(int groupId, string userId);
        Task<bool> IsMemberGroupAsync(int learnId, int groupId, string userId);
        Task<bool> SharedWithAsync(int learnId, string userId);
        Task<List<Learn>> UserLearnsAsync(string userId);
        Task<List<Learn>> GroupLearnsAsync(int groupId);
        Task<List<User>> SharedUsersAsync(int learnId);
        Task<List<SourceLore>> GetSourceLoresAsync();
    }
}
