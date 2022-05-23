using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities.WebModel
{
    public class Document
    {
        public string? Name { get; set; }

        public byte[]? FileContent { get; set; }
    }
}
