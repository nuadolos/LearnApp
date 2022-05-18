using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface IShareNoteRepo : IRepo<ShareNote>
    {
        Task<List<User>> GetUsersAsync(int noteId);
        Task<List<Note>> GetNotesAsync(string userId);
    }
}
