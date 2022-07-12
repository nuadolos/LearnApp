using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Models
{
    public class RequestNoteModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Link { get; set; } = null!;
        public bool IsVisible { get; set; }
        public Guid NoteTypeGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
