using LearnEF.Entities.Base;
using LearnEF.Entities.IdentityModel;
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
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display(Name = "URL материала")]
        [Required(ErrorMessage = "Поле \"URL материала\" пустое")]
        [StringLength(1000)]
        public string? Link { get; set; }

        [Display(Name = "Дата создания")]
        [Required(ErrorMessage = "Поле \"Дата создания\" пустое")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Дата прочтения")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? DateReading { get; set; }

        [Display(Name = "URL картинки")]
        public string? Image { get; set; }

        [Display(Name = "Ресурс")]
        [ForeignKey(nameof(SourceLoreId))]
        public int? SourceLoreId { get; set; }

        [Display(Name = "Принадлежит")]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Display(Name = "Изучен?")]
        [Required(ErrorMessage = "Поле \"Изучен?\" пустое")]
        public bool IsStudying { get; set; }

        public User? User { get; set; }

        public SourceLore? SourceLore { get; set; }

        [InverseProperty(nameof(Learn))]
        public List<ShareLearn>? ShareLearn { get; } = new List<ShareLearn>();

        [InverseProperty(nameof(Learn))]
        public List<GroupLearn>? GroupLearn { get; } = new List<GroupLearn>();

        [InverseProperty(nameof(Learn))]
        public List<LearnDocuments>? LearnDocuments { get; } = new List<LearnDocuments>();
    }
}
