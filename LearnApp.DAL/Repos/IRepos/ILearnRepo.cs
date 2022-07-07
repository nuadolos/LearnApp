using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface ILearnRepo : IRepo<Learn>
    {
        Task<List<Learn>> GetGroupLearnsAsync(Guid groupGuid);
        Task<Group?> GetGroupAsync(Guid groupGuid);
        Task<GroupUser?> GetGroupUserAsync(Guid groupGuid, Guid userGuid);
        Task<Learn?> GetLearnAsync(Guid learnGuid);
        Task<bool> IsMemberGroupAsync(Guid groupGuid, Guid userGuid);
        Task<bool> CanChangeLearnAsync(Guid groupGuid, Guid userGuid);
        Task<string> CreateFullLearnAsync(Learn learn, List<LearnDoc> documents);
        Task<string> DeleteAllDataLearnAsync(Guid learnGuid);
    }
}
