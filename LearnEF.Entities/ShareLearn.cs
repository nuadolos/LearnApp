using LearnEF.Entities.Base;
using LearnEF.Entities.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    public class ShareLearn : EntityBase
    {
        [Display(Name = "Материал")]
        [ForeignKey(nameof(LearnId))]
        public int? LearnId { get; set; }

        [Display(Name = "Пользователь")]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Display(Name = "Имеет доступ к изменению?")]
        public bool CanChange { get; set; }

        public Learn? Learn { get; set; }
        public User? User { get; set; }
    }
}
