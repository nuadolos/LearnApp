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
    [Table("Notes")]
    public partial class Note : EntityBase
    {
        [StringLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Unicode(false)]
        public string Link { get; set; } = null!;

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; }

        public bool IsVisible { get; set; }

        public string NoteTypeCode { get; set; } = null!;
        [ForeignKey(nameof(NoteTypeCode))]
        public NoteType NoteType { get; set; } = null!;

        public Guid UserGuid { get; set; }
        [ForeignKey(nameof(UserGuid))]
        public User User { get; set; } = null!;

        [InverseProperty(nameof(Note))]
        public ICollection<ShareNote> ShareNotes { get; set; } = new HashSet<ShareNote>();

        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(pr => pr.CreateDate)
                    .HasDefaultValueSql("(getdate())");
            });

            return modelBuilder;
        }
    }
}
