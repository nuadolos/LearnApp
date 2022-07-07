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
    [Table("GroupTypes")]
    [Index(nameof(Name), IsUnique = true)]
    public class GroupType : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(GroupType))]
        public ICollection<Group> Groups { get; } = new HashSet<Group>();
    }
}
