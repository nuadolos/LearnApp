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

        //[Column(TypeName = "nvarchar(MAX)")]
        public string? Description { get; set; }

        public string Link { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public bool IsVisible { get; set; }

        [ForeignKey(nameof(NoteTypeGuid))]
        public Guid NoteTypeGuid { get; set; }

        [ForeignKey(nameof(UserGuid))]
        public Guid UserGuid { get; set; }

        public User User { get; set; } = null!;

        public NoteType NoteType { get; set; } = null!;

        [InverseProperty(nameof(Note))]
        public ICollection<ShareNote> ShareNotes { get; } = new HashSet<ShareNote>();

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
