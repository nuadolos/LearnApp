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
    [Table("SourceLore")]
    public partial class SourceLore : EntityBase
    {
        [Required(ErrorMessage = "Поле \"Наименование\" пустое")]
        [Display(Name = "Наименование")]
        [StringLength(20)]
        public string? Name { get; set; }

        [InverseProperty(nameof(SourceLore))]
        public List<Note>? Note { get; } = new List<Note>();
    }
}
