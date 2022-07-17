using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Repos
{
    public class AttachRepo : BaseRepo<Attach>, IAttachRepo
    {
        public AttachRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Attach>> GetLearnAttachesAsync(Guid leardGuid) =>
            await Context.Attaches.Include(at => at.User).Where(at => at.LearnGuid == leardGuid).ToListAsync();

        public async Task<Attach?> GetAttachAsync(Guid leardGuid, Guid userGuid) =>
            await Context.Attaches.FirstOrDefaultAsync(at => at.LearnGuid == leardGuid && at.UserGuid == userGuid);
    }
}
