using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IAttachRepo : IRepo<Attach>
    {
        Task<List<Attach>> GetLearnAttachesAsync(Guid leardGuid);
        Task<Attach?> GetAttachAsync(Guid leardGuid, Guid userGuid);
    }
}
