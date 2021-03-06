using LearnApp.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Groups")]
    [Index(nameof(InviteCode), nameof(AdminCode), IsUnique = true)]
    public partial class Group : EntityBase
    {
        [StringLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid InviteCode { get; set; }
        public Guid AdminCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        public bool IsVisible { get; set; }

        public string GroupTypeCode { get; set; } = null!;
        [ForeignKey(nameof(GroupTypeCode))]
        public GroupType GroupType { get; set; } = null!;

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; } = null!;

        [InverseProperty(nameof(Group))]
        public ICollection<GroupUser> GroupUsers { get; set; } = new HashSet<GroupUser>();

        [InverseProperty(nameof(Group))]
        public ICollection<Learn> Learns { get; set; } = new HashSet<Learn>();

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(pr => pr.CreateDate)
                    .HasDefaultValueSql("(getdate())");
                entity.Property(pr => pr.InviteCode)
                    .HasDefaultValueSql("(newid())");
                entity.Property(pr => pr.AdminCode)
                    .HasDefaultValueSql("(newid())");
            });

            return modelBuilder;
        }
    }
}
