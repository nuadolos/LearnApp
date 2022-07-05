using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    public partial class Attach
    {
        [NotMapped]
        public string? UserName { get; set; }

        [NotMapped]
        public bool? IsAttached { get; set; }

        [NotMapped]
        public string? HasRated
        {
            get => Rating switch
            {
                0 => "Не поставлена",
                _ => Rating.ToString()
            };
        }
    }
}
