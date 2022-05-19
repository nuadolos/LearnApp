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
    [Table("Note")]
    public partial class Note : EntityBase
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display(Name = "Описание")]
        [StringLength(600)]
        public string? Description { get; set; }

        [Display(Name = "Ссылка на ресурс")]
        [Required(ErrorMessage = "Поле \"Ссылка на ресурс\" пустое")]
        [StringLength(1000)]
        public string? Link { get; set; }

        [Display(Name = "Дата создания")]
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Ресурс")]
        [Required]
        [ForeignKey(nameof(SourceLoreId))]
        public int? SourceLoreId { get; set; }

        [Display(Name = "Принадлежит")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        public User? User { get; set; }

        public SourceLore? SourceLore { get; set; }

        [InverseProperty(nameof(Note))]
        public List<ShareNote>? ShareNote { get; } = new List<ShareNote>();
    }
}
