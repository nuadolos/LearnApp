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
    [Table("ShareNote")]
    public class ShareNote : EntityBase
    {
        [ForeignKey(nameof(Note))]
        public int? NoteId { get; set; }

        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        public bool CanChange { get; set; }

        public User? User { get; set; }
        public Note? Note { get; set; }
    }
}
