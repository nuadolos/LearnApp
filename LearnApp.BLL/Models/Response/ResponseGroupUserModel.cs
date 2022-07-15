using LearnApp.DAL.Entities.Selects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LearnApp.BLL.Models.Response
{
    public class ResponseGroupUserModel
    {
        public string RoleName { get; set; }
        public int UserCount { get; set; }
        public List<UserViewData> Users { get; set; }
    }
}
