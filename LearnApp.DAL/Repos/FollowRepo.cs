using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.IRepos;

namespace LearnApp.DAL.Repos
{
    public class FollowerRepo : BaseRepo<Follower>, IFollowRepo
    {
        public FollowerRepo() : base()
        { }

        public FollowerRepo(LearnContext context) : base(context)
        { }

        public async Task<List<User>> GetFollowingAsync(Guid userGuid)
        {
            List<User> myFollowing = new List<User>();

            await Context.Follow
                .Include(f => f.TrackedUser)
                .Where(f => f.SubscribeUserGuid == userGuid)
                .ForEachAsync(f => myFollowing.Add(f.TrackedUser));

            return myFollowing;
        }

        public async Task<List<User>> GetFollowersAsync(Guid userGuid)
        {
            List<User> myFollowers = new List<User>();

            await Context.Follow
                .Include(f => f.SubscribeUser)
                .Where(f => f.TrackedUserGuid == userGuid)
                .ForEachAsync(f => myFollowers.Add(f.SubscribeUser));

            return myFollowers;
        }

        public async Task<bool> IsFollowingAsync(Guid subUserGuid, Guid trackUserGuid) =>
            await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserGuid == subUserGuid && f.TrackedUserGuid == trackUserGuid) != null;

        public async Task<string> FollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            var follower = await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserGuid == subUserGuid && f.TrackedUserGuid == trackUserGuid);

            if (follower != null)
                return "Вы уже подписаны на этого пользователя";

            follower = new Follower
            {
                SubscribeUserGuid = subUserGuid,
                TrackedUserGuid = trackUserGuid
            };

            try
            {
                await AddAsync(follower);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> UnfollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            var follower = await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserGuid == subUserGuid && f.TrackedUserGuid == trackUserGuid);

            if (follower == null)
                return "Вы не подписаны на этого пользователя";

            try
            {
                await DeleteAsync(follower);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
