using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface IGroupUserRepo : IRepo<GroupUser>
    {
        List<User> GetGroupUsers(int groupId);
        Task<string> UserRoleInGroup(int groupId, string userId);
        Task<string> JoinOpenGroupAsync(int groupId, string userId);
        Task<string> AcceptedInviteAsync(string inviteId, string userId);
        Task<string> KickUserAsync(int groupId, string userId);
    }
}
