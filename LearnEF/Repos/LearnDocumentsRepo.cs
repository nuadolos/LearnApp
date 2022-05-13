using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Repos
{
    public class LearnDocumentsRepo : BaseRepo<LearnDocuments>, ILearnDocumentsRepo
    {
        public async Task<List<LearnDocuments>> GetDocumentsAsync(int learnId) =>
            await Context.LearnDocuments.Where(doc => doc.LearnId == learnId).ToListAsync();
    }
}
