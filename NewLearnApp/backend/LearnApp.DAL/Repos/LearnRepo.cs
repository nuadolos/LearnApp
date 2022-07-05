using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.Base;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class LearnRepo : BaseRepo<Learn>, ILearnRepo
    {
        public LearnRepo() : base()
        { }

        public LearnRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Learn>> GetGroupLearnsAsync(int groupId) =>
            await Context.Learn.Where(l => l.GroupId == groupId).ToListAsync();

        public async Task<Group?> GetGroupAsync(int groupId) =>
            await Context.Group.FirstOrDefaultAsync(g => g.Id == groupId);

        public async Task<GroupUser?> GetGroupUserAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(gu => gu.GroupId == groupId && gu.UserId == userId);

        public async Task<Learn?> GetLearnAsync(int learnId) =>
            await Context.Learn.FirstOrDefaultAsync(l => l.Id == learnId);

        public async Task<bool> IsMemberGroupAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                gu => gu.GroupId == groupId && gu.UserId == userId) != null;

        public async Task<bool> CanChangeLearnAsync(int groupId, string userId) =>
            await Context.GroupUser.FirstOrDefaultAsync(
                    gu => gu.GroupId == groupId && gu.UserId == userId && gu.GroupRoleId != 1) != null;

        public async Task<string> CreateFullLearnAsync(Learn learn, List<LearnDocuments>? documents)
        {
            try
            {
                await AddAsync(learn);
                
                if (documents != null)
                {
                    var findLearn = await Context.Learn.OrderByDescending(l => l.Id).FirstOrDefaultAsync(
                        l => l.Title == learn.Title && l.UserId == learn.UserId && l.GroupId == learn.GroupId);

                    if (findLearn == null)
                        return "Не удалось найти задание, к которому вы хотите привязать документы";

                    foreach (var document in documents)
                    {
                        document.LearnId = findLearn.Id;
                    }

                    await Context.LearnDocuments.AddRangeAsync(documents);

                    await SaveChangesAsync();
                }
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> DeleteAllDataLearnAsync(int learnId)
        {
            var learn = await Context.Learn.FirstOrDefaultAsync(l => l.Id == learnId);

            if (learn == null)
                return "Искомое задание отсутствует";

            var learnDocs = Context.LearnDocuments.Where(ld => ld.LearnId == learn.Id);
            var attaches = Context.Attaches.Where(at => at.LearnId == learn.Id);

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
