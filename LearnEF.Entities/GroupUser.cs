using LearnEF.Entities.Base;
using LearnEF.Entities.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    [Table("GroupUser")]
    public class GroupUser : EntityBase
    {
        [Display(Name = "Группа")]
        [ForeignKey(nameof(GroupId))]
        public int? GroupId { get; set; }

        [Display(Name = "Пользователь")]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Display(Name = "Роль")]
        [ForeignKey(nameof(GroupRoleId))]
        public int? GroupRoleId { get; set; }

        public Group? Group { get; set; }
        public User? User { get; set; }
        public GroupRole? GroupRole { get; set; }
    }
}
