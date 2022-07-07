using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface INoteTypeRepo : IRepo<NoteType>
    {
        bool ContainedInNote(Guid noteTypeGuid);
    }
}
