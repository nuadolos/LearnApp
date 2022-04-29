#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class UserChangePassword
    {
        [Required(ErrorMessage = "Поле \"Email\" пустое")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле \"Пароль\" пустое")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле \"Подтверждение пароля\" пустое")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Поле \"Секретный код\" пустое")]
        [Display(Name = "Секретный код")]
        public string CodeInMessage { get; set; }

        [Display(Name = "Количество попыток")]
        public int CountAttempts { get; set; }
    }
}
