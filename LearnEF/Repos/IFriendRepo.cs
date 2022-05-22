using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IFriendRepo : IRepo<Follow>
    {
        Task<List<User>> GetFriendsAsync(string userId);
    }
}
