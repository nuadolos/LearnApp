using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface IGroupRepo : IRepo<Group>
    { 
        Task<List<Group>> GetVisibleGroupsAsync();
        Task<List<Group>> GetUserGroupsAsync(string userId);
        Task<List<Group>> GetMemberGroupsAsync(string userId);
        Task<bool> IsCreatorAsync(int groupId, string userId);
        Task<bool> IsMemberAsync(int groupId, string userId);
        Task<string> DeleteAllDataAboutGroupAsync(int groupId);
    }
}
