using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface ISourceLoreRepo : IRepo<SourceLore>
    {
        bool ContainedInNote(int id);
    }
}
