using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo() : base()
        { }

        public UserRepo(LearnContext context) : base(context)
        { }

        public async Task<User?> GetByGuidAsync(Guid userGuid) =>
            await Context.User.FirstOrDefaultAsync(u => u.Guid == userGuid);

        public async Task<User?> GetByLoginAsync(string login) =>
            await Context.User.FirstOrDefaultAsync(u => u.Login == login);

        public async Task<string> DeleteFullDataUserAsync(Guid userGuid)
        {
            var subUser = Context.Follow.Where(f => f.SubscribeUserGuid == userGuid);
            var trackUser = Context.Follow.Where(f => f.TrackedUserGuid == userGuid);
            var shareNote = Context.ShareNote.Where(sn => sn.UserGuid == userGuid);
            var attach = Context.Attaches.Where(at => at.UserGuid == userGuid);
            var groupUser = Context.GroupUser.Where(at => at.UserGuid == userGuid);

            Context.Follow.RemoveRange(subUser);
            Context.Follow.RemoveRange(trackUser);
            Context.ShareNote.RemoveRange(shareNote);
            Context.Attaches.RemoveRange(attach);
            Context.GroupUser.RemoveRange(groupUser);

            var learn = Context.Learn
                .Include(l => l.LearnDocs)
                .Include(l => l.Attaches)
                .Where(l => l.UserGuid == userGuid);

            if (learn != null)
            {
                foreach (var item in learn)
                {
                    Context.LearnDocuments.RemoveRange(item.LearnDocs);
                    Context.Attaches.RemoveRange(item.Attaches);
                }

                Context.Learn.RemoveRange(learn);
            }

            try
            {
                await SaveChangesAsync();

                var group = Context.Group
                    .Include(g => g.Learns)
                    .Include(g => g.GroupUsers)
                    .Where(g => g.UserGuid == userGuid);

                foreach (var item in group)
                {
                    Context.Learn.RemoveRange(item.Learns);
                    Context.GroupUser.RemoveRange(item.GroupUsers);
                }

                await SaveChangesAsync();

                Context.Group.RemoveRange(group);

                var note = Context.Note
                    .Include(g => g.ShareNotes)
                    .Where(g => g.UserGuid == userGuid);

                foreach (var item in note)
                {
                    Context.ShareNote.RemoveRange(item.ShareNotes);
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
