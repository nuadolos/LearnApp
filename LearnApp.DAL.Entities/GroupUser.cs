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
    [Table("GroupUsers")]
    public class GroupUser : EntityBase
    {
        public Guid GroupGuid { get; set; }
        [ForeignKey(nameof(GroupGuid))]
        public Group Group { get; set; }

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; }

        public string GroupRoleCode { get; set; }
        [ForeignKey(nameof(GroupRoleCode))]
        public GroupRole GroupRole { get; set; }
    }
}
