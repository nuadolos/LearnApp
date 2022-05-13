using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    public partial class Group
    {
        [NotMapped]
        public string? TypeName { get; set; }
    }
}
