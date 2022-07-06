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
    [Table("Group")]
    public partial class Group : EntityBase
    {
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        public Guid CodeAdmin { get; set; }

        public Guid CodeInvite { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        public bool IsVisible { get; set; }

        [ForeignKey(nameof(GroupTypeGuid))]
        public Guid GroupTypeGuid { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        public GroupType GroupType { get; set; } = null!;

        public User User { get; set; } = null!;

        [InverseProperty(nameof(Group))]
        public ICollection<GroupUser>? GroupUsers { get; } = new HashSet<GroupUser>();

        [InverseProperty(nameof(Group))]
        public ICollection<Learn>? Learns { get; } = new HashSet<Learn>();
    }
}
