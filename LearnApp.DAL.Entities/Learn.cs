using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Learn")]
    public partial class Learn : EntityBase
    {
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(600)]
        public string? Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime Deadline { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        [ForeignKey(nameof(GroupGuid))]
        public Guid GroupGuid { get; set; }

        public User User { get; set; } = null!;

        public Group Group { get; set; } = null!;

        [InverseProperty(nameof(Learn))]
        public ICollection<LearnDoc> LearnDocs { get; } = new HashSet<LearnDoc>();

        [InverseProperty(nameof(Learn))]
        public ICollection<Attach> Attaches { get; } = new HashSet<Attach>();
    }
}
