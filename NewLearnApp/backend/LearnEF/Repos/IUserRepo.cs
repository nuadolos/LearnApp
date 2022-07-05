using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IUserRepo : IRepo<User>
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByLoginAsync(string login);
    }
}
