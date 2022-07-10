using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.DAL.Exceptions;

namespace LearnApp.DAL.Repos
{
    public class ShareNoteRepo : BaseRepo<ShareNote>, IShareNoteRepo
    {
        public ShareNoteRepo() : base()
        { }

        public ShareNoteRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Note>> GetNotesAsync(Guid userGuid)
        {
            List<Note> userNotes = new List<Note>();

            await Context.ShareNote
                .Include(sl => sl.Note)
                .Where(sl => sl.UserGuid == userGuid)
                .ForEachAsync(sl => userNotes.Add(sl.Note));

            return userNotes;
        }

        public async Task<List<User>> GetUsersAsync(Guid noteGuid)
        {
            List<User> noteUsers = new List<User>();

            await Context.ShareNote
                .Include(sn => sn.User)
                .Where(sn => sn.NoteGuid == noteGuid)
                .ForEachAsync(sn => noteUsers.Add(sn.User));

            return noteUsers;
        }

        public async Task<ShareNote?> GetShareNoteAsync(Guid noteGuid, Guid userGuid) =>
            await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteGuid == noteGuid && sn.UserGuid == userGuid);
    }
}
