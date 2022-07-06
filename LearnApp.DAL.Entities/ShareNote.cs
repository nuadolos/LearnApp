using LearnApp.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace LearnApp.DAL.Entities
{
    [Table("ShareNote")]
    public class ShareNote : EntityBase
    {
        [ForeignKey(nameof(NoteGuid))]
        public Guid NoteGuid { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        public User User { get; set; }
        public Note Note { get; set; }
    }
}
