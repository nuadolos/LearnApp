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
    [Table("Note")]
    public partial class Note : EntityBase
    {
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(600)]
        public string? Description { get; set; }

        [StringLength(1000)]
        public string Link { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [ForeignKey(nameof(SourceLoreGuid))]
        public Guid SourceLoreGuid { get; set; }

        public bool IsVisible { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        public User User { get; set; } = null!;

        public NoteType SourceLore { get; set; } = null!;

        [InverseProperty(nameof(Note))]
        public ICollection<ShareNote>? ShareNotes { get; } = new HashSet<ShareNote>();
    }
}
