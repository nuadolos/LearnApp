using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("GroupType")]
    public class GroupType : EntityBase
    {
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Поле \"Наименование\" пустое")]
        [StringLength(20)]
        public string? Name { get; set; }

        [InverseProperty(nameof(GroupType))]
        public List<Group>? Group { get; } = new List<Group>();
    }
}
