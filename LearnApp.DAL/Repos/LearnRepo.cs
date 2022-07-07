using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
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

        public async Task<Group?> GetGroupAsync(Guid groupGuid) =>
            await Context.Group.FirstOrDefaultAsync(g => g.Guid == groupGuid);

        public async Task<GroupUser?> GetGroupUserAsync(Guid groupGuid, Guid userGuid) =>
            await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid);

        public async Task<Learn?> GetLearnAsync(Guid  learnGuid) =>
            await Context.Learn.FirstOrDefaultAsync(l => l.Guid == learnGuid);

        public async Task<bool> IsMemberGroupAsync(Guid groupGuid, Guid userGuid) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid) != null;

        public async Task<bool> CanChangeLearnAsync(Guid groupGuid, Guid userGuid) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                    gu => gu.GroupGuid == groupGuid && gu.UserGuid == userGuid /*&& gu.GroupRoleId != 1*/) != null;

        public async Task<string> CreateFullLearnAsync(Learn learn, List<LearnDoc>? documents)
        {
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    document.Learn = learn;
                }

                await Context.LearnDocuments.AddRangeAsync(documents);
            }

            try
            { 
                await AddAsync(learn);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> DeleteAllDataLearnAsync(Guid learnGuid)
        {
            var learn = await Context.Learn.FirstOrDefaultAsync(l => l.Guid == learnGuid);

            if (learn == null)
                return "Искомое задание отсутствует";

            var learnDocs = Context.LearnDocuments.Where(ld => ld.LearnGuid == learn.Guid);
            var attaches = Context.Attaches.Where(at => at.LearnGuid == learn.Guid);

            Context.LearnDocuments.RemoveRange(learnDocs);
            Context.Attaches.RemoveRange(attaches);

            try
            {
                await SaveChangesAsync();

                await DeleteAsync(learn);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
