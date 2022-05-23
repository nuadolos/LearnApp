using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Entities.Base;
using LearnEF.Entities.ErrorModel;
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

            Context.LearnDocuments.RemoveRange(learnDocs);

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
