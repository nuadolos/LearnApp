using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.ErrorModel;

namespace LearnApp.DAL.Repos
{
    public class LearnDocumentsRepo : BaseRepo<LearnDocuments>, ILearnDocumentsRepo
    {
        public LearnDocumentsRepo() : base()
        { }

        public LearnDocumentsRepo(LearnContext context) : base(context)
        { }

        public async Task<List<LearnDocuments>> GetDocumentsAsync(int learnId) =>
            await Context.LearnDocuments.Where(doc => doc.LearnId == learnId).ToListAsync();

        public async Task<string> LoadAsync(LearnDocuments document)
        {
            var learnDoc = await Context.LearnDocuments.FirstOrDefaultAsync(
                ld => ld.LearnId == document.LearnId && ld.Name == document.Name 
                    && ld.FileContent.Length == document.FileContent.Length);

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
