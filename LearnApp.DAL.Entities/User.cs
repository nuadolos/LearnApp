using LearnApp.DAL.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Users")]
    [Index(nameof(Login), nameof(Salt), IsUnique = true)]
    public partial class User : EntityBase
    {
        [Unicode(false)]
        [StringLength(50)]
        public string Login { get; set; } = null!;

        [JsonIgnore]
        [Unicode(false)]
        [StringLength(450)]
        public string PasswordHash { get; set; } = null!;
        
        [JsonIgnore]
        [Unicode(false)]
        [StringLength(450)]
        public string Salt { get; set; } = null!;
        
        [StringLength(40)]
        public string Surname { get; set; } = null!;

        [StringLength(40)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        [StringLength(6)]
        public string? Code { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime? CodeTimeBlock { get; set; }

        public string UserRoleCode { get; set; } = null!;
        [ForeignKey(nameof(UserRoleCode))]
        public UserRole UserRole { get; set; } = null!;

        [InverseProperty(nameof(User))]
        public ICollection<Learn> Learns { get; set; } = new HashSet<Learn>();

        [InverseProperty(nameof(User))]
        public ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        [InverseProperty(nameof(User))]
        public ICollection<Attach> Attaches { get; set; } = new HashSet<Attach>();

        [InverseProperty(nameof(User))]
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();

        [InverseProperty(nameof(User))]
        public ICollection<ShareNote> ShareNotes { get; set; } = new HashSet<ShareNote>();

        [InverseProperty(nameof(User))]
        public ICollection<GroupUser> GroupUsers { get; set; } = new HashSet<GroupUser>();

        [InverseProperty("SubscribeUser")]
        public ICollection<Follower> SubscribeUsers { get; set; } = new HashSet<Follower>();

        [InverseProperty("TrackedUser")]
        public ICollection<Follower> TrackedUsers { get; set; } = new HashSet<Follower>();
    }
}
