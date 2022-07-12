using LearnApp.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Learns")]
    public partial class Learn : EntityBase
    {
        [StringLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime Deadline { get; set; }

        public Guid GroupGuid { get; set; }
        [ForeignKey(nameof(GroupGuid))]
        public Group Group { get; set; } = null!;

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; } = null!;

        [InverseProperty(nameof(Learn))]
        public ICollection<LearnDoc> LearnDocs { get; } = new HashSet<LearnDoc>();

        [InverseProperty(nameof(Learn))]
        public ICollection<Attach> Attaches { get; } = new HashSet<Attach>();

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Learn>(entity =>
            {
                entity.Property(pr => pr.CreateDate)
                    .HasDefaultValueSql("(getdate())");
            });

            return modelBuilder;
        }
    }
}
