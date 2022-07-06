using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LearnApp.DAL.Entities
{
    [Table("Follow")]
    public class Follow : EntityBase
    {
        [ForeignKey(nameof(SubscribeUserGuid))]
        public Guid SubscribeUserGuid { get; set; }

        [ForeignKey(nameof(TrackedUserGuid))]
        public Guid TrackedUserGuid { get; set; }

        [Column(TypeName = "date")]
        public DateTime FollowDate { get; set; }

        public User SubscribeUser { get; set; }
        public User TrackedUser { get; set; }
    }
}
