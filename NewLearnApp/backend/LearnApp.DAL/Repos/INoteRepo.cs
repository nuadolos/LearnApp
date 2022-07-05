using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public interface INoteRepo : IRepo<Note>
    {
        Task<List<Note>> GetUserNotesAsync(string userId);
        Task<List<SourceLore>> GetSourceLoresAsync();
        Task<bool> SharedWithAsync(int noteId, string userId);
        Task<bool> CanChangeNoteAsync(int noteId, string userId);
        Task<string> DeleteNoteAsync(int noteId, byte[] timestamp);
    }
}
