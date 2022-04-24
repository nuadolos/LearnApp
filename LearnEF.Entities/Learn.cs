using LearnEF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    [Table("Learn")]
    public partial class Learn : EntityBase
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [Display(Name = "URL материала")]
        [Required]
        [StringLength(1000)]
        public string? Link { get; set; }
        [Display(Name = "Дата создания")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Дата прочтения")]
        [DataType(DataType.Date)]
        public DateTime? DateReading { get; set; }
        [Display(Name = "URL картинки")]
        public string? Image { get; set; }
        [Display(Name = "Ресурс")]
        [ForeignKey(nameof(SourceLoreId))]
        public int? SourceLoreId { get; set; }
        [Display(Name = "Изучен?")]
        [Required]
        public bool IsStudying { get; set; }
        public SourceLore? SourceLore { get; set; }
    }
}
