using LearnApp.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
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
    [Table("GroupRoles")]
    [Index(nameof(Name), IsUnique = true)]
    public class GroupRole : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(GroupRole))]
        public ICollection<GroupUser> GroupUsers { get; set; } = new HashSet<GroupUser>();
    }
}
