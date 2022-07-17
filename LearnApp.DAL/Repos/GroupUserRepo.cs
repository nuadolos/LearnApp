using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Entities.Selects;

namespace LearnApp.DAL.Repos
{
    public class GroupUserRepo : BaseRepo<GroupUser>, IGroupUserRepo
    {
        public GroupUserRepo(LearnContext context) : base(context)
        { }

        public async Task<User> GetGroupCreatorAsync(Guid groupGuid) =>
            (await Context.Group
                .Include(g => g.User)
                .FirstOrDefaultAsync(g => g.Guid == groupGuid))?.User ?? new User();

        public async Task<List<UserViewData>> GetGroupUsersAsync(Guid groupGuid) =>
            await Context.GroupUser
                .Include(gu => gu.User)
                .Include(gu => gu.GroupRole)
                .Where(gu => gu.GroupGuid == groupGuid)
                .Select(gu => new UserViewData
                {
                    Login = gu.User.Login,
                    Surname = gu.User.Surname,
                    Name = gu.User.Name,
                    Middlename = gu.User.Middlename,
                    GroupRoleName = gu.GroupRole.Code
                })
                .ToListAsync();

        public async Task<Group?> GetGroupByGuidAsync(Guid groupGuid) =>
            await Context.Group
                .Include(g => g.GroupUsers)
                .FirstOrDefaultAsync(g => g.Guid == groupGuid);

        public async Task<Group?> GetGroupByInviteCodeAsync(Guid inviteGuid) =>
            await Context.Group
                .Include(g => g.GroupUsers)
                .FirstOrDefaultAsync(g => g.InviteCode == inviteGuid || g.AdminCode == inviteGuid);

        public async Task<GroupUser?> GetGroupUserAsync(Guid groupGuid, Guid userGuid) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid);
    }
}
