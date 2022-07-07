using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;
using LearnApp.DAL.Repos.IRepos;

namespace LearnApp.DAL.Repos
{
    public class NoteTypeRepo : BaseRepo<NoteType>, INoteTypeRepo
    {
        public NoteTypeRepo() : base()
        { }
        public NoteTypeRepo(LearnContext context) : base(context)
        { }

        public bool ContainedInNote(Guid noteTypeGuid)
        {
            foreach (var note in Context.Note)
            {
                if (note.NoteTypeGuid == noteTypeGuid)
                    return true;
            }

            return false;
        }
    }
}
