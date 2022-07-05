using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo() : base()
        { }

        public UserRepo(LearnContext context) : base(context)
        { }

        public async Task<User?> GetByIdAsync(string id) =>
            await Context.User.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByLoginAsync(string login) =>
            await Context.User.FirstOrDefaultAsync(u => u.Login == login);
    }
}
