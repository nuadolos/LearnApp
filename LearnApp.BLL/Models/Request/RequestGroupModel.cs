using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Models.Request
{
    public class RequestGroupModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsVisible { get; set; }
        public string GroupTypeCode { get; set; } = null!;
        public Guid UserGuid { get; set; }
    }
}
