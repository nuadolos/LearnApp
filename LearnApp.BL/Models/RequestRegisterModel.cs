#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BL.Models
{
    public class RequestRegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
    }
}
