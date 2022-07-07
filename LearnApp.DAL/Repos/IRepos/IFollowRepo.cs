using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IFollowRepo : IRepo<Follower>
    {
        Task<List<User>> GetFollowingAsync(Guid userGuid);
        Task<List<User>> GetFollowersAsync(Guid userGuid);
        Task<bool> IsFollowingAsync(Guid subUserGuid, Guid trackUserGuid);
        Task<string> FollowAsync(Guid subUserGuid, Guid trackUserGuid);
        Task<string> UnfollowAsync(Guid subUserGuid, Guid trackUserGuid);
    }
}
