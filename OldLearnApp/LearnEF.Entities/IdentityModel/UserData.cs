using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public class UserData
    {
        [Required]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Поле \"Email\" пустое")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле \"Пароль\" пустое")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле \"Фамилия\" пустое")]
        [StringLength(40, ErrorMessage = "Фамилия не должна быть больше 40 символов")]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Поле \"Имя\" пустое")]
        [StringLength(40, ErrorMessage = "Имя не должно быть больше 40 символов")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Display(Name = "Имеет доступ?")]
        public bool Enabled { get; set; }
    }
}
