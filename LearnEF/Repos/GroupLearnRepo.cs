using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Repos
{
    public class GroupLearnRepo : BaseRepo<GroupLearn>, IGroupLearnRepo
    {
        public List<Learn> GetGroupLearns(int groupId)
        {
            List<Learn> groupLearns = new List<Learn>();

            Context.GroupLearn
                .Include(gl => gl.Learn)
                .Where(gl => gl.GroupId == groupId)
                .ForEachAsync(gl => groupLearns.Add(gl.Learn))
                .Wait();

            return groupLearns;
        }
    }
}
