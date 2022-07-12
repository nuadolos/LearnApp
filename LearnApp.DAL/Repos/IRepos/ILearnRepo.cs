using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface ILearnRepo : IRepo<Learn>
    {
        Task<List<Learn>> GetGroupLearnsAsync(Guid groupGuid);
        Task<List<Learn>> GetCreatorLearnsAsync(Guid userGuid);
        Task<Learn?> GetLearnByGuidAsync(Guid learnGuid);
        Task<bool> IsMemberGroupAsync(Guid groupGuid, Guid userGuid);
        Task<bool> CanChangeLearnAsync(Guid groupGuid, Guid userGuid);
        Task<string> CreateFullLearnAsync(Learn learn, List<LearnDoc> documents);
        Task<string> DeleteAllDataLearnAsync(Guid learnGuid);
    }
}
