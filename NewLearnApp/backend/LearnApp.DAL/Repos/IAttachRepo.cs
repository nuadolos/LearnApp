using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface IAttachRepo : IRepo<Attach>
    {
        Task<List<Attach>> GetLearnAttachesAsync(int leardId);
        Task<Attach?> GetAttachAsync(int leardId, string userId);
    }
}
