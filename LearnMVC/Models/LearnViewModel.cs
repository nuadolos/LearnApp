using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.WebModel
{
    public class LearnViewModel
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display(Name = "Описание")]
        [StringLength(600)]
        public string? Description { get; set; }

        [Display(Name = "Дата создания")]
        [Required(ErrorMessage = "Поле \"Дата создания\" пустое")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Дата сдачи")]
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Документы")]
        public IFormFileCollection? FileContent { get; set; }
    }
}
