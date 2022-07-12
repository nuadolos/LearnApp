using LearnApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Models
{
    public class RequestLearnModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public Guid GroupGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
