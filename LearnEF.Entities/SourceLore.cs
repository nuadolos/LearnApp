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
    [Table("SourceLore")]
    public partial class SourceLore : EntityBase
    {
        [Required(ErrorMessage = "Поле \"Наименование\" пустое")]
        [Display(Name = "Наименование")]
        public string? Name { get; set; }

        [InverseProperty(nameof(SourceLore))]
        public List<Learn>? Learn { get; set; } = new List<Learn>();
    }
}
