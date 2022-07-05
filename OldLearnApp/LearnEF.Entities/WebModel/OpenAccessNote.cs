#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.WebModel
{
    public class OpenAccessNote
    {
        public int NoteId { get; set; }

        [Required(ErrorMessage = "Поле \"Email\" пустое")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Разрешить доступ к редактированию?")]
        public bool CanChange { get; set; }
    }
}
