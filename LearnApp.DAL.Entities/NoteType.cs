using LearnApp.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("NoteTypes")]
    [Index(nameof(Code), IsUnique = true)]
    public partial class NoteType
    {
        [Key]
        [StringLength(100)]
        public string Code { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }

        [InverseProperty(nameof(NoteType))]
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();
    }
}
