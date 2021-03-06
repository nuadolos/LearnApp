using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface ILearnDocumentsRepo : IRepo<LearnDocuments>
    {
        Task<List<LearnDocuments>> GetDocumentsAsync(int learnId);
        Task<string> LoadAsync(LearnDocuments document);
    }
}
