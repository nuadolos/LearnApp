using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public class NoteRepo : BaseRepo<Note>, INoteRepo
    {
        public async Task<List<Note>> GetUserNotes(string userId) =>
            await Context.Note.Where(n => n.UserId == userId).ToListAsync();

        public async Task<List<SourceLore>> GetSourceLoresAsync() =>
            await Context.SourceLore.ToListAsync();

        public async Task<bool> SharedWithAsync(int noteId, string userId) =>
            await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId) != null;

        public async Task<bool> CanChangeLearnAsync(int noteId, string userId) =>
            (await Context.ShareNote.FirstOrDefaultAsync(
                sn => sn.NoteId == noteId && sn.UserId == userId))?.CanChange == true;

        public async Task<string> DeleteLearnAsync(int noteId, byte[] timestamp)
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
