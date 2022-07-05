using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IAttachRepo : IRepo<Attach>
    {
        Task<List<Attach>> GetLearnAttachesAsync(int leardId);
        Task<Attach?> GetAttachAsync(int leardId, string userId);
    }
}
