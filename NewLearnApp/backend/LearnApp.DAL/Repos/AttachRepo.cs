using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
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
        public AttachRepo() : base()
        { }

        public AttachRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Attach>> GetLearnAttachesAsync(int leardId) =>
            await Context.Attaches.Include(at => at.User).Where(at => at.LearnId == leardId).ToListAsync();

        public async Task<Attach?> GetAttachAsync(int leardId, string userId) =>
            await Context.Attaches.FirstOrDefaultAsync(at => at.LearnId == leardId && at.UserId == userId);
    }
}
