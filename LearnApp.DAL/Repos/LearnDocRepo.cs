using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.IRepos;

namespace LearnApp.DAL.Repos
{
    public class LearnDocRepo : BaseRepo<LearnDoc>, ILearnDocRepo
    {
        public LearnDocRepo() : base()
        { }

        public LearnDocRepo(LearnContext context) : base(context)
        { }

        public async Task<List<LearnDoc>> GetDocumentsAsync(Guid learnGuid) =>
            await Context.LearnDocuments.Where(doc => doc.LearnGuid == learnGuid).ToListAsync();

        public async Task<string> LoadAsync(LearnDoc document)
        {
            var learnDoc = await Context.LearnDocuments.FirstOrDefaultAsync(
                ld => ld.LearnGuid == document.LearnGuid && ld.FileName == document.FileName 
                    && ld.FilePath == document.FilePath);

            if (learnDoc != null)
                return "Этот файл уже был загружен";
            
            try
            {
                await AddAsync(document);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
