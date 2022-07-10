using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BL.Models
{
    public class RequestGroupModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsVisible { get; set; }
        public Guid GroupTypeGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
