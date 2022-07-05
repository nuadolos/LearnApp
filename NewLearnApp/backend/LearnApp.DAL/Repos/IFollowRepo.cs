using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
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
