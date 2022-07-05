using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface IUserRepo : IRepo<User>
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByLoginAsync(string login);
    }
}
