using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class UserRegister
    {
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string? PasswordConfirm { get; set; }
    }
}
