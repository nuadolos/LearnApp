using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnApp.DAL.Entities.Base
{
    [Index(nameof(Guid), IsUnique = true)]
    public class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}
