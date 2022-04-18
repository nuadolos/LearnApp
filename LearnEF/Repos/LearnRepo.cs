using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Repos.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public class LearnRepo : BaseRepo<Learn>, ILearnRepo
    {
        public LearnRepo() : base()
        { }

        public LearnRepo(LearnContext context) : base(context)
        { }

        public List<Learn> GetRelatedData() =>
            Context.Learn.FromSqlRaw("SELECT * FROM Learn").Include(lore => lore.SourceLore).ToList();

        public List<SourceLore> GetSourceLores() =>
            Context.SourceLore.ToList();

        public List<Learn> Search(string searchString) =>
            Context.Learn.Where(l => EF.Functions.Like(l.Title, $"%{searchString}%")).ToList();
    }
}
