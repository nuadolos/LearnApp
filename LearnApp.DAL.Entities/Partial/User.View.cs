using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    public partial class User
    {
        [NotMapped]
        public int? GroupRoleId { get; set; }

        [NotMapped]
        public int? GroupId { get; set; }

        [NotMapped]
        public int? NoteId { get; set; }

        [NotMapped]
        public string? CanChangeNote { get; set; }

        [NotMapped]
        public bool? FollowingHim { get; set; }
    }
}
