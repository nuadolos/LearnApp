using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities.WebModel
{
    public class FullLearn
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? Deadline { get; set; }

        public List<Document>? Files { get; set; }

        public int? GroupId { get; set; }
    }
}
