using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnEF.Context;
using LearnEF.Entities.ErrorModel;

namespace LearnEF.Repos
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
                ld => ld.Name == document.Name && ld.FileContent.Length == document.FileContent.Length);

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
