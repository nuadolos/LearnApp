using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.ErrorModel;

namespace LearnApp.DAL.Repos
{
    public class GroupRepo : BaseRepo<Group>, IGroupRepo
    {
        public GroupRepo() : base()
        { }

        public GroupRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Group>> GetUserGroupsAsync(string userId) =>
             await Context.Group.Where(g => g.UserId == userId).ToListAsync();

        public async Task<List<Group>> GetMemberGroupsAsync(string userId)
        {
            List<Group> groups = new List<Group>();

            await Context.GroupUser
                .Include(gu => gu.Group)
                .Where(gu => gu.UserId == userId)
                .ForEachAsync(gu => groups.Add(gu.Group));

            return groups;
        }

        public async Task<List<Group>> GetVisibleGroupsAsync() =>
            await Context.Group.Where(g => g.IsVisible == true).ToListAsync();

        public async Task<bool> IsCreatorAsync(int groupId, string userId) =>
            await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == userId) != null;

        public async Task<bool> IsMemberAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupId == groupId && gu.UserId == userId) != null;

        public async Task<string> DeleteAllDataAboutGroupAsync(int groupId)
        {
            var group = await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                return "Искомая группа не существует";

            var members = Context.GroupUser.Where(gu => gu.GroupId == groupId);
            var learns = Context.Learn.Where(l => l.GroupId == groupId);

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
