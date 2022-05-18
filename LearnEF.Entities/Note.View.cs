using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    public partial class Note
    {
        [NotMapped]
        [Display(Name = "Источник")]
        public string? LoreName { get; set; }
    }
}
