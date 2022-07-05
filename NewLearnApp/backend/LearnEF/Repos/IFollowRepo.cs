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
    public interface IFollowRepo : IRepo<Follow>
    {
        Task<List<User>> GetFollowingAsync(string userId);
        Task<List<User>> GetFollowersAsync(string userId);
        Task<bool> IsFollowingAsync(string subUserId, string trackUserId);
        Task<string> FollowAsync(string subUserId, string trUserId);
        Task<string> UnfollowAsync(string subUserId, string trUserId);
        Task<string> DeleteFullDataUserAsync(string userId);
    }
}
