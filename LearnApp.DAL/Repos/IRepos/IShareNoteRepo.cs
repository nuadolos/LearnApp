using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface IShareNoteRepo : IRepo<ShareNote>
    {
        Task<List<User>> GetUsersAsync(Guid noteGuid);
        Task<List<Note>> GetNotesAsync(Guid userGuid);
        Task<ShareNote?> GetShareNoteAsync(Guid noteGuid, Guid userGuid);
    }
}
