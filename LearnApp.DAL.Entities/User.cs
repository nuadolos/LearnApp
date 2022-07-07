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
        [StringLength(50)]
        public string Login { get; set; } = null!;

        [JsonIgnore]
        [StringLength(450)]
        public string PasswordHash { get; set; } = null!;
        
        [JsonIgnore]
        [StringLength(450)]
        public string Salt { get; set; } = null!;
        
        [StringLength(40)]
        public string Surname { get; set; } = null!;

        [StringLength(40)]
        public string Name { get; set; } = null!;

        [StringLength(40)]
        public string Middlename { get; set; } = null!;

        [StringLength(6)]
        public string? Code { get; set; }
        public DateTime? CodeTimeBlock { get; set; }

        [ForeignKey(nameof(UserRoleGuid))]
        public Guid UserRoleGuid { get; set; }

        #region Navigation props

        public UserRole UserRole { get; set; } = null!;

        [InverseProperty(nameof(User))]
        public ICollection<Learn> Learns { get; set; } = new HashSet<Learn>();

        [InverseProperty(nameof(User))]
        public ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        [InverseProperty(nameof(User))]
        public ICollection<Attach> Attaches { get; } = new HashSet<Attach>();

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

        #endregion
    }
}
