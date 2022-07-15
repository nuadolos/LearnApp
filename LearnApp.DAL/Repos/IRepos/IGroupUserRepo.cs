using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.Selects;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IGroupUserRepo : IRepo<GroupUser>
    {
        Task<User> GetGroupCreatorAsync(Guid groupGuid);
        Task<List<UserViewData>> GetGroupUsersAsync(Guid groupGuid);
        Task<Group?> GetGroupByGuidAsync(Guid groupGuid);
        Task<Group?> GetGroupByInviteCodeAsync(Guid inviteGuid);
        Task<GroupUser?> GetGroupUserAsync(Guid groupGuid, Guid userGuid);
    }
}
