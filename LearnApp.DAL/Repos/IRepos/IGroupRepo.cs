using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IGroupRepo : IRepo<Group>
    {
        Task<List<Group>> GetVisibleGroupsAsync();
        Task<List<Group>> GetUserGroupsAsync(Guid userGuid);
        Task<Group?> GetGroupAsync(Guid groupGuid, Guid userGuid);
        Task<bool> IsCreatorAsync(Guid groupGuid, Guid userGuid);
        Task<bool> IsMemberAsync(Guid groupGuid, Guid userGuid);
        Task<string> DeleteAllDataAboutGroupAsync(Guid groupGuid);
    }
}
