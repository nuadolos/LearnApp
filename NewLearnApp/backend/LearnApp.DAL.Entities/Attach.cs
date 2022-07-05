using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Attach")]
    public partial class Attach : EntityBase
    {
        [Required]
        [ForeignKey(nameof(LearnId))]
        public int? LearnId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Display(Name = "День сдачи")]
        [Required]
        [Column(TypeName = "date")]
        public DateTime? AttachmentDate { get; set; }

        [Display(Name = "Оценка")]
        [Required]
        public int? Rating { get; set; }

        [Display(Name = "Имя файла")]
        public string? FileName { get; set; }

        public byte[]? FileContent { get; set; }

        public Learn? Learn { get; set; }

        public User? User { get; set; }
    }
}
