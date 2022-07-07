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
        [ForeignKey(nameof(SubscribeUserGuid))]
        public Guid SubscribeUserGuid { get; set; }

        [ForeignKey(nameof(TrackedUserGuid))]
        public Guid TrackedUserGuid { get; set; }

        public DateTime FollowDate { get; set; }

        public User SubscribeUser { get; set; }
        public User TrackedUser { get; set; }

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
