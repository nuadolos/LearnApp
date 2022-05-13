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
        public async Task<List<Learn>> GetGroupLearnsAsync(int groupId)
        {
            List<Learn> groupLearns = new List<Learn>();

            await Context.GroupLearn
                .Include(gl => gl.Learn)
                .Where(gl => gl.GroupId == groupId)
                .ForEachAsync(gl => groupLearns.Add(gl.Learn));

            return groupLearns;
        }
    }
}
