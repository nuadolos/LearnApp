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
    [Table("Attaches")]
    public partial class Attach : EntityBase
    {
        [Required]
        [StringLength(150)]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public byte? Rating { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime AttachmentDate { get; set; }

        public Guid LearnGuid { get; set; }
        [ForeignKey(nameof(LearnGuid))]
        public Learn Learn { get; set; }

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; }

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attach>(entity =>
            {
                //entity.HasOne(e => e.Learn)
                //    .WithMany(e => e.Attaches)
                //    .HasForeignKey(fk => fk.LearnGuid);
                //entity.HasOne(e => e.User)
                //    .WithMany(e => e.Attaches)
                //    .HasForeignKey(fk => fk.UserGuid);
                entity.Property(pr => pr.AttachmentDate)
                    .HasDefaultValueSql("(getdate())");
            });

            return modelBuilder;
        }
    }
}
