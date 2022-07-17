using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.DAL.Exceptions;

namespace LearnApp.DAL.Repos
{
    public class FollowerRepo : BaseRepo<Follower>, IFollowerRepo
    {
        public FollowerRepo(LearnContext context) : base(context)
        { }

        public async Task<List<User>> GetFollowingAsync(Guid userGuid)
        {
            List<User> myFollowing = new List<User>();

            await Context.Follower
                .Include(f => f.TrackedUser)
                .Where(f => f.SubscribeUserGuid == userGuid)
                .ForEachAsync(f => myFollowing.Add(f.TrackedUser));

            return myFollowing;
        }

        public async Task<List<User>> GetFollowersAsync(Guid userGuid)
        {
            List<User> myFollowers = new List<User>();

            await Context.Follower
                .Include(f => f.SubscribeUser)
                .Where(f => f.TrackedUserGuid == userGuid)
                .ForEachAsync(f => myFollowers.Add(f.SubscribeUser));

            return myFollowers;
        }

        public async Task<Follower?> GetFollowerAsync(Guid subUserGuid, Guid trackUserGuid) =>
            await Context.Follower.FirstOrDefaultAsync(
                f => f.SubscribeUserGuid == subUserGuid && f.TrackedUserGuid == trackUserGuid);
    }
}
