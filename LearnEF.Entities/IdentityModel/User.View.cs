using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public partial class User
    {
        [NotMapped]
        public int? GroupRoleId { get; set; }
    }
}
