using LearnEF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    [Table("GroupRole")]
    public class GroupRole : EntityBase
    {
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Поле \"Наименование\" пустое")]
        [StringLength(20)]
        public string? Name { get; set; }

        [InverseProperty(nameof(GroupRole))]
        public List<GroupUser>? GroupUser { get; } = new List<GroupUser>();
    }
}
