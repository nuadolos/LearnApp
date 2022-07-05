using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface IShareNoteRepo : IRepo<ShareNote>
    {
        List<User> GetUsersAsync(int noteId);
        Task<List<Note>> GetNotesAsync(string userId);
        Task<string> OpenAccessAsync(int noteId, string userId, bool canChange);
        Task<string> BlockAccessAsync(int noteId, string userId);
    }
}
