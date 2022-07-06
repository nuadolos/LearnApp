using LearnApp.DAL.Entities.Base;
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
    [Table("LearnDoc")]
    public class LearnDoc : EntityBase
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        [ForeignKey(nameof(LearnGuid))]
        public Guid LearnGuid { get; set; }

        public Learn Learn { get; set; }
    }
}
