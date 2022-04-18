using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public interface ILearnRepo : IRepo<Learn>
    {
        List<Learn> Search(string searchString);
        List<Learn> GetRelatedData();
        List<SourceLore> GetSourceLores();
    }
}
