using LearnEF.Context;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
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
