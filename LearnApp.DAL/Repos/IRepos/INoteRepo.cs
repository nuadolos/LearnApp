using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos.IRepos
{
    public interface INoteRepo : IRepo<Note>
    {
        Task<List<Note>> GetUserNotesAsync(Guid noteGuid);
        Task<List<NoteType>> GetNoteTypesAsync();
        Task<bool> SharedWithAsync(Guid noteGuid, Guid userGuid);
        Task<string> DeleteNoteAsync(Guid noteGuid, byte[] timestamp);
    }
}
