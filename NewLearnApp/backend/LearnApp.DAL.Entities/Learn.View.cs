using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    public partial class Learn
    {
        [NotMapped]
        public string? UserRoleName { get; set; }
    }
}
