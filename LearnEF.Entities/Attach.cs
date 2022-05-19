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
    [Table("Attach")]
    public class Attach : EntityBase
    {
        [Required]
        [ForeignKey(nameof(LearnId))]
        public int? LearnId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime? AttachmentDate { get; set; }

        public byte[]? FileContent { get; set; }

        public Learn? Learn { get; set; }

        public User? User { get; set; }
    }
}
