using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IGroupRepo : IRepo<Group>
    { 
        Task<List<Group>> GetVisibleGroupsAsync();
        Task<bool> IsCreatorAsync(int groupId, string userId);
        Task<bool> IsMemberAsync(int groupId, string userId);
    }
}
