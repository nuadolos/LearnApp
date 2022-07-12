using LearnApp.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace LearnApp.DAL.Entities
{
    [Table("ShareNotes")]
    public class ShareNote : EntityBase
    {
        public Guid NoteGuid { get; set; }
        [ForeignKey(nameof(NoteGuid))]
        public Note Note { get; set; }

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; }
    }
}
