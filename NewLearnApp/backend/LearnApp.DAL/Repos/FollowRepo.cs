using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.ErrorModel;

namespace LearnApp.DAL.Repos
{
    public class FollowRepo : BaseRepo<Follow>, IFollowRepo
    {
        public FollowRepo() : base()
        { }

        public FollowRepo(LearnContext context) : base(context)
        { }

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

        public async Task<string> DeleteFullDataUserAsync(string userId)
        {
            var subUser = Context.Follow.Where(f => f.SubscribeUserId == userId);
            var trackUser = Context.Follow.Where(f => f.TrackedUserId == userId);
            var shareNote = Context.ShareNote.Where(sn => sn.UserId == userId);
            var attach = Context.Attaches.Where(at => at.UserId == userId);
            var groupUser = Context.GroupUser.Where(at => at.UserId == userId);

            Context.Follow.RemoveRange(subUser);
            Context.Follow.RemoveRange(trackUser);
            Context.ShareNote.RemoveRange(shareNote);
            Context.Attaches.RemoveRange(attach);
            Context.GroupUser.RemoveRange(groupUser);

            var learn = Context.Learn
                .Include(l => l.LearnDocuments)
                .Include(l => l.Attach)
                .Where(l => l.UserId == userId);

            if (learn != null)
            {
                foreach (var item in learn)
                {
                    Context.LearnDocuments.RemoveRange(item.LearnDocuments);
                    Context.Attaches.RemoveRange(item.Attach);
                }
            }

            try
            {
                await SaveChangesAsync();

                Context.Learn.RemoveRange(learn);

                await SaveChangesAsync();

                var group = Context.Group
                    .Include(g => g.Learn)
                    .Include(g => g.GroupUser)
                    .Where(g => g.UserId == userId);

                foreach (var item in group)
                {
                    Context.Learn.RemoveRange(item.Learn);
                    Context.GroupUser.RemoveRange(item.GroupUser);
                }

                await SaveChangesAsync();

                Context.Group.RemoveRange(group);

                var note = Context.Note
                    .Include(g => g.ShareNote)
                    .Where(g => g.UserId == userId);

                foreach (var item in note)
                {
                    Context.ShareNote.RemoveRange(item.ShareNote);
                }

                await SaveChangesAsync();

                Context.Note.RemoveRange(note);

                await SaveChangesAsync();
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
