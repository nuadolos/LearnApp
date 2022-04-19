using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class UserChangePassword
    {
        public string? Id { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
    }
}
