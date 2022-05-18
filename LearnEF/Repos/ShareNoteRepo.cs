using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Repos
{
    public class ShareNoteRepo : BaseRepo<ShareNote>, IShareNoteRepo
    {
        public ShareNoteRepo() : base()
        { }

        public ShareNoteRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Note>> GetNotesAsync(string userId)
        {
            List<Note> userNotes = new List<Note>();

            await Context.ShareNote
                .Include(sl => sl.NoteId)
                .Where(sl => sl.UserId == userId)
                .ForEachAsync(sl => userNotes.Add(sl.Note));

            return userNotes;
        }

        public async Task<List<User>> GetUsersAsync(int noteId)
        {
            List<User> noteUsers = new List<User>();

            await Context.ShareNote
                .Include(sl => sl.User)
                .Where(sl => sl.NoteId == noteId)
                .ForEachAsync(sl => noteUsers.Add(sl.User));

            return noteUsers;
        }
    }
}
