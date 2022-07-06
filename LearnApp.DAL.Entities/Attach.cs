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
    [Table("Attach")]
    public partial class Attach : EntityBase
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int Rating { get; set; }


        [Column(TypeName = "date")]
        public DateTime AttachmentDate { get; set; }

        public Guid LearnGuid { get; set; }

        public Guid UserGuid { get; set; }

        public Learn Learn { get; set; }

        public User User { get; set; }

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attach>(entity =>
            {
                entity.HasOne(e => e.Learn)
                    .WithMany(e => e.Attaches)
                    .HasForeignKey(fk => fk.LearnGuid);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Attaches)
                    .HasForeignKey(fk => fk.UserGuid);
            });

            return modelBuilder;
        }
    }
}
