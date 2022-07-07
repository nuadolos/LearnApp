using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IGroupUserRepo : IRepo<GroupUser>
    {
        Task<List<User>> GetGroupUsersAsync(Guid groupGuid);
        Task<string> UserRoleInGroupAsync(Guid groupGuid, Guid userGuid);
        Task<string> JoinOpenGroupAsync(Guid groupGuid, Guid userGuid);
        Task<string> AcceptedInviteAsync(Guid inviteGuid, Guid userGuid);
        Task<string> KickUserAsync(Guid groupGuid, Guid userGuid);
    }
}
