using LearnEF.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearnEF.Entities.IdentityModel
{
    public partial class User : EntityBase
    {
        [Required]
        [StringLength(450)]
        public new string? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Login { get; set; }
        [Required]
        [JsonIgnore]
        [StringLength(450)]
        public string? PasswordHash { get; set; }
        [Required]
        [JsonIgnore]
        [StringLength(450)]
        public string? Salt { get; set; }
        [Required]
        [StringLength(40)]
        public string? Surname { get; set; }
        [Required]
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(6)]
        public string? Code { get; set; }
        public DateTime? CodeTimeBlock { get; set; }

        [InverseProperty(nameof(User))]
        public List<Learn>? Learn { get; set; } = new List<Learn>();

        [InverseProperty(nameof(User))]
        public List<Group>? Group { get; set; } = new List<Group>();

        [InverseProperty(nameof(User))]
        public List<Attach>? Attach { get; } = new List<Attach>();

        [InverseProperty(nameof(User))]
        public List<Note>? Note { get; set; } = new List<Note>();

        [InverseProperty(nameof(User))]
        public List<ShareNote>? ShareNote { get; set; } = new List<ShareNote>();

        [InverseProperty(nameof(User))]
        public List<GroupUser>? GroupUser { get; set; } = new List<GroupUser>();

        [InverseProperty("SubscribeUser")]
        public List<Follow>? SubscribeUser { get; set; } = new List<Follow>();

        [InverseProperty("TrackedUser")]
        public List<Follow>? TrackedUser { get; set; } = new List<Follow>();
    }
}
