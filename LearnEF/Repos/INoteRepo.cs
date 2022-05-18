using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface INoteRepo : IRepo<Note>
    {
        Task<List<Note>> GetUserNotes(string userId);
        Task<List<SourceLore>> GetSourceLoresAsync();
        Task<bool> SharedWithAsync(int noteId, string userId);
        Task<bool> CanChangeLearnAsync(int noteId, string userId);
        Task<string> DeleteLearnAsync(int noteId, byte[] timestamp);
    }
}
