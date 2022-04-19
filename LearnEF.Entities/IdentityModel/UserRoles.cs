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
        public string? Id { get; set; }
        public string? Email { get; set; }
        public List<IdentityRole>? AllRoles { get; set; }
        public IList<string>? Roles { get; set; }

        public UserRoles()
        {
            AllRoles = new List<IdentityRole>();
            Roles = new List<string>();
        }
    }
}
