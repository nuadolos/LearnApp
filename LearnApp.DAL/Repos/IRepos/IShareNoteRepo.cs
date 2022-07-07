using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IShareNoteRepo : IRepo<ShareNote>
    {
        Task<List<User>> GetUsersAsync(Guid noteGuid);
        Task<List<Note>> GetNotesAsync(Guid userGuid);
        Task<string> OpenAccessAsync(Guid noteGuid, Guid userGuid);
        Task<string> BlockAccessAsync(Guid noteGuid, Guid userGuid);
    }
}
