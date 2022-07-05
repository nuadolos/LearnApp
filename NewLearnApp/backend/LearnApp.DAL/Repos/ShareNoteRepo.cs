using Microsoft.EntityFrameworkCore;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;

namespace LearnApp.DAL.Repos
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

        public List<User> GetUsersAsync(int noteId)
        {
            List<User> noteUsers = new List<User>();

            var shareNotes = Context.ShareNote
                .Include(sl => sl.User)
                .Where(sl => sl.NoteId == noteId);

            foreach (var item in shareNotes.AsParallel())
            {
                item.User.NoteId = item.NoteId;
                item.User.CanChangeNote = item.CanChange ? "Имеется" : "Отсутствует";

                noteUsers.Add(item.User);
            }

            return noteUsers;
        }

        public async Task<string> OpenAccessAsync(int noteId, string userId, bool canChange)
        {
            var shareNote = await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId);

            if (shareNote != null)
                return "Вы уже поделились заметкой с этим пользователем";

            shareNote = new ShareNote {
                NoteId = noteId,
                UserId = userId,
                CanChange = canChange
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

        public async Task<string> BlockAccessAsync(int noteId, string userId)
        {
            var shareNote = await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId);

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
