using LearnApp.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
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
    [Table("Followers")]
    public class Follower : EntityBase
    {
        public Guid SubscribeUserGuid { get; set; }
        [ForeignKey(nameof(SubscribeUserGuid))]
        public User SubscribeUser { get; set; }

        public Guid TrackedUserGuid { get; set; }
        [ForeignKey(nameof(TrackedUserGuid))]
        public User TrackedUser { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime FollowDate { get; set; }

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follower>(entity =>
            {
                entity.Property(pr => pr.FollowDate)
                    .HasDefaultValueSql("(getdate())");
            });

            return modelBuilder;
        }
    }
}
