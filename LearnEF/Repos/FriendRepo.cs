using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Repos
{
    public class FriendRepo : BaseRepo<Friend>, IFriendRepo
    {
        public List<User> GetFriends(string userId)
        {
            List<User> friends = new List<User>();

            Context.Friend
                .Include(f => f.AcceptedUser)
                .Where(f => f.SentUserId == userId)
                .ForEachAsync(f => friends.Add(f.AcceptedUser))
                .Wait();

            Context.Friend
                .Include(f => f.SentUser)
                .Where(f => f.AcceptedUserId == userId)
                .ForEachAsync(f => friends.Add(f.SentUser))
                .Wait();

            return friends;
        }
    }
}
