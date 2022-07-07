using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.IRepos;

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

        public async Task<string> OpenAccessAsync(Guid noteGuid, Guid userGuid)
        {
            var shareNote = await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteGuid == noteGuid && sn.UserGuid == userGuid);

            if (shareNote != null)
                return "Вы уже поделились заметкой с этим пользователем";

            shareNote = new ShareNote {
                NoteGuid = noteGuid,
                UserGuid = userGuid
            };

            try
            {
                await AddAsync(shareNote);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public async Task<string> BlockAccessAsync(Guid noteGuid, Guid userGuid)
        {
            var shareNote = await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteGuid == noteGuid && sn.UserGuid == userGuid);

            if (shareNote == null)
                return "Вы не делились заметкой с этим пользователем";

            try
            {
                await DeleteAsync(shareNote);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
