using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    public class UserRole : EntityBase
    {
        [Required(ErrorMessage = "Поле \"Наименование\" пустое")]
        [StringLength(50, ErrorMessage = "Наименование должно не превышать 50 символов")]
        [Display(Name = "Наименование")]
        public string? Name { get; set; }
    }
}
