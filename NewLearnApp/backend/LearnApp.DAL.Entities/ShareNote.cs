using LearnApp.DAL.Entities.Base;
using LearnApp.DAL.Entities.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("ShareNote")]
    public class ShareNote : EntityBase
    {
        [Required]
        [ForeignKey(nameof(Note))]
        public int? NoteId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        public bool CanChange { get; set; }

        public User? User { get; set; }
        public Note? Note { get; set; }
    }
}
