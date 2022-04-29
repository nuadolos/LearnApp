#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class OnlyEmail
    {
        [Required(ErrorMessage = "Поле \"Email\" пустое")]
        public string Email { get; set; }
    }
}
