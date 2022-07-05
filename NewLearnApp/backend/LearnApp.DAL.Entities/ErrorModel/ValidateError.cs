using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities.ErrorModel
{
    public class ValidateError
    {
        public string Message { get; set; }

        public ValidateError(string message) =>
            Message = message;
    }
}
