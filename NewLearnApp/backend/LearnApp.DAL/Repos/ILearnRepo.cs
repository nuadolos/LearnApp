using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface ILearnRepo : IRepo<Learn>
    {
        Task<List<Learn>> GetGroupLearnsAsync(int groupId);
        Task<Group?> GetGroupAsync(int groupId);
        Task<GroupUser?> GetGroupUserAsync(int groupId, string userId);
        Task<Learn?> GetLearnAsync(int learnId);
        Task<bool> IsMemberGroupAsync(int groupId, string userId);
        Task<bool> CanChangeLearnAsync(int groupId, string userId);
        Task<string> CreateFullLearnAsync(Learn learn, List<LearnDocuments> documents);
        Task<string> DeleteAllDataLearnAsync(int learnId);
    }
}
