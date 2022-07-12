using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LearnApp.BLL.Models
{
    public class RequestRemoveDataModel
    {
        public Guid Guid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
