using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IFollowerRepo : IRepo<Follower>
    {
        Task<List<User>> GetFollowingAsync(Guid subUserGuid);
        Task<List<User>> GetFollowersAsync(Guid trackUserGuid);
        Task<Follower?> GetFollowerAsync(Guid subUserGuid, Guid trackUserGuid);
    }
}
