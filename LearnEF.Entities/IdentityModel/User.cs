﻿#nullable disable
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(40)]
        public string Surname { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
