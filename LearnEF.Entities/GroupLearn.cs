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
    [Table("GroupLearn")]
    public class GroupLearn : EntityBase
    {
        [Display(Name = "Группа")]
        [ForeignKey(nameof(GroupId))]
        public int? GroupId { get; set; }

        [Display(Name = "Материал")]
        [ForeignKey(nameof(LearnId))]
        public int? LearnId { get; set; }

        public Group? Group { get; set; }
        public Learn? Learn { get; set; }
    }
}
