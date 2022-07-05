using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Learn")]
    public partial class Learn : EntityBase
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display(Name = "Описание")]
        [StringLength(600)]
        public string? Description { get; set; }

        [Display(Name = "Дата создания")]
        [Required(ErrorMessage = "Поле \"Дата создания\" пустое")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Дата сдачи")]
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? Deadline { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [Required]
        [ForeignKey(nameof(GroupId))]
        public int? GroupId { get; set; }

        public User? User { get; set; }

        public Group? Group { get; set; }

        [InverseProperty(nameof(Learn))]
        public List<LearnDocuments>? LearnDocuments { get; } = new List<LearnDocuments>();

        [InverseProperty(nameof(Learn))]
        public List<Attach>? Attach { get; } = new List<Attach>();
    }
}
