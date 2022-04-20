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
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Link { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateReading { get; set; }
        public string? Image { get; set; }
        [ForeignKey(nameof(SourceLoreId))]
        public int? SourceLoreId { get; set; }
        [Required]
        public bool? IsStudying { get; set; }
        public SourceLore? SourceLore { get; set; }
    }
}
