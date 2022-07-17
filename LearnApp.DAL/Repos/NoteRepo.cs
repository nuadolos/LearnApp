using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.DAL.Repos
{
    public class NoteRepo : BaseRepo<Note>, INoteRepo
    {
        public NoteRepo(LearnContext context) : base(context)
        { }

        public async Task<List<Note>> GetUserNotesAsync(Guid userGuid) =>
            await Context.Note.Where(n => n.UserGuid == userGuid).ToListAsync();

        public async Task<List<NoteType>> GetNoteTypesAsync() =>
            await Context.NoteType.ToListAsync();

        public async Task<bool> IsCreator(Guid noteGuid, Guid userGuid) =>
            await Context.Note.FirstOrDefaultAsync(
                n => n.Guid == noteGuid && n.UserGuid == userGuid) != null;

        public async Task<bool> SharedWithAsync(Guid noteGuid, Guid userGuid) =>
            await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteGuid == noteGuid && sn.UserGuid == userGuid) != null;

        public async Task<string> DeleteAllDataAboutNoteAsync(Guid noteGuid, byte[] timestamp)
        {
            var note = await Context.Note.Include(n => n.ShareNotes)
                .FirstAsync(n => n.Guid == noteGuid && n.Timestamp == timestamp);
            note.ShareNotes.Clear();

            try
            {
                await DeleteAsync(note);
            }
            catch (DbMessageException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
