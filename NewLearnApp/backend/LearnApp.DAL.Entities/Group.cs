using LearnApp.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Entities
{
    [Table("Group")]
    public partial class Group : EntityBase
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(50, ErrorMessage = "Название максимум содержит 50 символов")]
        public string? Name { get; set; }

        [Display(Name = "Описание")]
        [StringLength(1000, ErrorMessage = "Описание максимум содержит 1000 символов")]
        public string? Description { get; set; }

        [Display(Name = "Код преподавателя")]
        [StringLength(200)]
        public string? CodeAdmin { get; set; }

        [Display(Name = "Код студента")]
        [StringLength(200)]
        public string? CodeInvite { get; set; }

        [Display(Name = "Дата создания")]
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Открытый?")]
        [Required]
        public bool IsVisible { get; set; }

        [Display(Name = "Тип группы")]
        [Required(ErrorMessage = "Поле \"Тип группы\" пустое")]
        [ForeignKey(nameof(GroupTypeId))]
        public int? GroupTypeId { get; set; }

        [Display(Name = "Принадлежит")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        public GroupType? GroupType { get; set; }

        public User? User { get; set; }

        [InverseProperty(nameof(Group))]
        public List<GroupUser>? GroupUser { get; } = new List<GroupUser>();

        [InverseProperty(nameof(Group))]
        public List<Learn>? Learn { get; } = new List<Learn>();
    }
}
