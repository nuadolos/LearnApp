using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface ILearnDocumentsRepo : IRepo<LearnDocuments>
    {
        Task<List<LearnDocuments>> GetDocumentsAsync(int learnId);
        Task<string> LoadAsync(LearnDocuments document);
    }
}
