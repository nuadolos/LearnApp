using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    public partial class Group
    {
        [NotMapped]
        [Display(Name = "Тип группы")]
        public string? TypeName
        {
            get => GroupTypeId switch
            {
                1 => "Равноправный",
                _ => "Класс"
            };
        }
    }
}
