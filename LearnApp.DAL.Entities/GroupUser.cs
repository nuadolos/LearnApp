using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LearnApp.DAL.Entities
{
    [Table("GroupUser")]
    public class GroupUser : EntityBase
    {
        [ForeignKey(nameof(GroupGuid))]
        public Guid GroupGuid { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        [ForeignKey(nameof(GroupRoleGuid))]
        public Guid GroupRoleGuid { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
        public GroupRole GroupRole { get; set; }
    }
}
