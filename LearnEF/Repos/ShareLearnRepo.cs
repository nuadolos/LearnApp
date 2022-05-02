using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos
{
    public class ShareLearnRepo : BaseRepo<ShareLearn>, IShareLearn
    {
        public ShareLearnRepo() : base()
        { }

        public ShareLearnRepo(LearnContext context) : base(context)
        { }

        public bool ContainedInLearn(string userId)
        {
            throw new NotImplementedException();
        }

        public bool ContainedInUser(int learnId)
        {
            throw new NotImplementedException();
        }
    }
}
