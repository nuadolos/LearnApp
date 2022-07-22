#nullable disable
using LearnApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Models.Request
{
    public class RequestRegisterModel
    {
        // todo: validate properties in server with DataAnnotations
        public string Login { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
    }
}
