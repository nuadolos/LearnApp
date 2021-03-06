using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Context;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.DAL.Exceptions;

namespace LearnApp.DAL.Repos
{
    public class GroupRepo : BaseRepo<Group>, IGroupRepo
    {
        public GroupRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Group>> GetVisibleGroupsAsync() =>
            await Context.Group.Where(g => g.IsVisible == true).ToListAsync();

        public async Task<List<Group>> GetUserGroupsAsync(Guid userGuid)
        {
            List<Group> groups = await Context.Group
                .Where(g => g.UserGuid == userGuid)
                .OrderBy(g => g.CreateDate)
                .ToListAsync();

            await Context.GroupUser
                .Include(gu => gu.Group)
                .Where(gu => gu.UserGuid == userGuid)
                .ForEachAsync(gu => groups.Add(gu.Group));

            return groups;
        }

        public async Task<Group?> GetGroupByGuidAsync(Guid groupGuid, Guid userGuid) =>
            await Context.Group
                .Include(g => g.GroupUsers
                    .FirstOrDefault(gu => gu.UserGuid == userGuid))
                .FirstOrDefaultAsync(g => g.Guid == groupGuid);

        public async Task<bool> IsCreatorAsync(Guid groupGuid, Guid userGuid) =>
            await Context.Group.FirstOrDefaultAsync(
                g => g.Guid == groupGuid && g.UserGuid == userGuid) != null;

        public async Task<bool> IsMemberAsync(Guid groupGuid, Guid userGuid) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid) != null;

        public async Task<string> DeleteAllDataAboutGroupAsync(Guid groupGuid)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.Guid == groupGuid);

            if (group == null)
                return "Искомая группа не существует";

            var members = Context.GroupUser.Where(gu => gu.GroupGuid == groupGuid);
            var learns = Context.Learn.Where(l => l.GroupGuid == groupGuid);

            if (members != null)
                Context.GroupUser.RemoveRange(members);

            if (learns != null)
                Context.Learn.RemoveRange(learns);

            try
            {
                await SaveChangesAsync();

                await DeleteAsync(group);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
