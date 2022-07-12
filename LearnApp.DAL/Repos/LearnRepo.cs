using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class LearnRepo : BaseRepo<Learn>, ILearnRepo
    {
        public LearnRepo() : base()
        { }

        public LearnRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Learn>> GetGroupLearnsAsync(Guid groupGuid) =>
            await Context.Learn.Where(l => l.GroupGuid == groupGuid).ToListAsync();

        public async Task<List<Learn>> GetCreatorLearnsAsync(Guid userGuid) =>
            await Context.Learn.Where(l => l.UserGuid == userGuid).ToListAsync();
        public async Task<Learn?> GetLearnByGuidAsync(Guid  learnGuid) =>
            await Context.Learn.Include(l => l.Group).ThenInclude(g => g.GroupUsers)
                .FirstOrDefaultAsync(l => l.Guid == learnGuid);
    }
}
