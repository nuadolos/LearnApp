using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface ILearnDocRepo : IRepo<LearnDoc>
    {
        Task<List<LearnDoc>> GetDocumentsAsync(Guid learnGuid);
        Task<string> LoadAsync(LearnDoc document);
    }
}
