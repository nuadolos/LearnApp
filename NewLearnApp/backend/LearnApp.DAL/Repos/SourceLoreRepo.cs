using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.Base;

namespace LearnApp.DAL.Repos
{
    public class SourceLoreRepo : BaseRepo<SourceLore>, ISourceLoreRepo
    {
        public SourceLoreRepo() : base()
        { }
        public SourceLoreRepo(LearnContext context) : base(context)
        { }

        public bool ContainedInNote(int id)
        {
            foreach (var note in Context.Note)
            {
                if (note.SourceLoreId == id)
                    return true;
            }

            return false;
        }
    }
}
