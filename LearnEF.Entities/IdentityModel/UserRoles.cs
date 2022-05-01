using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class UserRoles
    {
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public List<IdentityRole>? AllRoles { get; set; }
        public IList<string>? UsRoles { get; set; }
    }
}
