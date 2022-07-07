using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class NoteRepo : BaseRepo<Note>, INoteRepo
    {
        public NoteRepo() : base()
        { }

        public NoteRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Note>> GetUserNotesAsync(Guid userGuid) =>
            await Context.Note.Where(n => n.UserGuid == userGuid).ToListAsync();

        public async Task<List<NoteType>> GetNoteTypesAsync() =>
            await Context.NoteType.ToListAsync();

        public async Task<bool> SharedWithAsync(Guid noteGuid, Guid userGuid) =>
            await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteGuid == noteGuid && sn.UserGuid == userGuid) != null;

        public async Task<string> DeleteNoteAsync(Guid noteGuid, byte[] timestamp)
        {
            var shareNotes = Context.ShareNote.Where(sn => sn.NoteGuid == noteGuid);

            Context.ShareNote.RemoveRange(shareNotes);

            try
            {
                await SaveChangesAsync();

                await DeleteAsync(noteGuid, timestamp);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
