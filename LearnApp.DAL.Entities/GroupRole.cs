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
    [Table("GroupRole")]
    public class GroupRole : EntityBase
    {
        [StringLength(20)]
        public string Name { get; set; }

        [InverseProperty(nameof(GroupRole))]
        public ICollection<GroupUser> GroupUsers { get; } = new HashSet<GroupUser>();
    }
}
