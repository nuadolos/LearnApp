using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    public partial class Learn
    {
        [NotMapped]
        [Display(Name = "Статус")]
        public string? Studied
        {
            get => IsStudying ? "Изучен" : "Не изучен";
        }

        [NotMapped]
        public string? LoreName { get; set; }

        [NotMapped]
        public string? UserName { get; set; }

        //Новый синтаксис оператора switch
        //return SourceLoreId switch
        //{
        //    1 => "Тест1115",
        //    2 => "Тест2225",
        //    3 => "Тест3335",
        //    _ => "Неизвестно"
        //};
    }
}
