using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IUserRepo : IRepo<User>
    {
        Task<User?> GetByGuidAsync(Guid userGuid);
        Task<User?> GetByLoginAsync(string login);
        Task<string> DeleteFullDataUserAsync(Guid userGuid);
    }
}
