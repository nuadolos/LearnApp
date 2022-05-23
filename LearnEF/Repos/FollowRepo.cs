using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnEF.Entities.ErrorModel;

namespace LearnEF.Repos
{
    public class FollowRepo : BaseRepo<Follow>, IFollowRepo
    {
        public async Task<List<User>> GetFollowingAsync(string userId)
        {
            List<User> myFollowing = new List<User>();

            await Context.Follow
                .Include(f => f.TrackedUser)
                .Where(f => f.SubscribeUserId == userId)
                .ForEachAsync(f => myFollowing.Add(f.TrackedUser));

            return myFollowing;
        }

        public async Task<List<User>> GetFollowersAsync(string userId)
        {
            List<User> myFollowers = new List<User>();

            await Context.Follow
                .Include(f => f.SubscribeUser)
                .Where(f => f.TrackedUserId == userId)
                .ForEachAsync(f => myFollowers.Add(f.SubscribeUser));

            return myFollowers;
        }

        public async Task<bool> IsFollowingAsync(string subUserId, string trUserId) =>
            await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserId == subUserId && f.TrackedUserId == trUserId) != null;

        public async Task<string> FollowAsync(string subUserId, string trUserId)
        {
            var follow = await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserId == subUserId && f.TrackedUserId == trUserId);

            if (follow != null)
                return "Вы уже подписаны на этого пользователя";

            follow = new Follow {
                SubscribeUserId = subUserId,
                TrackedUserId = trUserId,
                FollowDate = DateTime.Now
            };

            try
            {
                await AddAsync(follow);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> UnfollowAsync(string subUserId, string trUserId)
        {
            var follow = await Context.Follow.FirstOrDefaultAsync(
                f => f.SubscribeUserId == subUserId && f.TrackedUserId == trUserId);

            if (follow == null)
                return "Вы не подписаны на этого пользователя";

            try
            {
                await DeleteAsync(follow);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
