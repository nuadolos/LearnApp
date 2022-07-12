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
    [Table("LearnDocs")]
    public class LearnDoc : EntityBase
    {
        [Required]
        [StringLength(150)]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public Guid LearnGuid { get; set; }
        [ForeignKey(nameof(LearnGuid))]
        public Learn Learn { get; set; }
    }
}
