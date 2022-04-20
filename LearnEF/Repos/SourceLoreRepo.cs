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
    public class SourceLoreRepo : BaseRepo<SourceLore>, ISourceLoreRepo
    {
        public SourceLoreRepo() : base()
        { }
        public SourceLoreRepo(LearnContext context) : base(context)
        { }

        public bool ContainedInLearn(int id)
        {
            foreach (var learn in Context.Learn)
            {
                if (learn.SourceLoreId == id)
                    return true;
            }

            return false;
        }
    }
}
