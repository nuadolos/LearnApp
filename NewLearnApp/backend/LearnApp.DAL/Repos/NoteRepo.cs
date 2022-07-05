using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.Base;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class NoteRepo : BaseRepo<Note>, INoteRepo
    {
        public NoteRepo() : base()
        { }

        public NoteRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Note>> GetUserNotesAsync(string userId) =>
            await Context.Note.Where(n => n.UserId == userId).ToListAsync();

        public async Task<List<SourceLore>> GetSourceLoresAsync() =>
            await Context.SourceLore.ToListAsync();

        public async Task<bool> SharedWithAsync(int noteId, string userId) =>
            await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId) != null;

        public async Task<bool> CanChangeNoteAsync(int noteId, string userId) =>
            (await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId))?.CanChange == true;

        public async Task<string> DeleteNoteAsync(int noteId, byte[] timestamp)
        {
            var shareNotes = Context.ShareNote.Where(sn => sn.NoteId == noteId);

            Context.ShareNote.RemoveRange(shareNotes);

            try
            {
                await SaveChangesAsync();

                await DeleteAsync(noteId, timestamp);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
