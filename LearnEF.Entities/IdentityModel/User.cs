﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public partial class User : IdentityUser
    {
        [Required]
        [StringLength(40)]
        public string? Surname { get; set; }
        [Required]
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(6)]
        public string? Code { get; set; }
        public DateTime? CodeTimeBlock { get; set; }

        [InverseProperty(nameof(User))]
        public List<Learn>? Learn { get; set; } = new List<Learn>();

        [InverseProperty(nameof(User))]
        public List<ShareLearn>? ShareLearn { get; set; } = new List<ShareLearn>();

        [InverseProperty(nameof(User))]
        public List<GroupUser>? GroupUser { get; set; } = new List<GroupUser>();

        [InverseProperty("SentUser")]
        public List<Friend>? SentUser { get; set; } = new List<Friend>();

        [InverseProperty("AcceptedUser")]
        public List<Friend>? AcceptedUser { get; set; } = new List<Friend>();
    }
}
