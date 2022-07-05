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
    [Table("Follow")]
    public class Follow : EntityBase
    {
        [Required]
        [ForeignKey(nameof(SubscribeUserId))]
        public string? SubscribeUserId { get; set; }

        [Required]
        [ForeignKey(nameof(TrackedUserId))]
        public string? TrackedUserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime? FollowDate { get; set; }

        public User? SubscribeUser { get; set; }
        public User? TrackedUser { get; set; }
    }
}
