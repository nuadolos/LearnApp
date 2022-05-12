using LearnEF.Entities.Base;
using LearnEF.Entities.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    [Table("Group")]
    public class Group : EntityBase
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле \"Название\" пустое")]
        [StringLength(50, ErrorMessage = "Название максимум содержит 50 символов")]
        public string? Name { get; set; }

        [Display(Name = "Описание")]
        [StringLength(1000, ErrorMessage = "Описание максимум содержит 1000 символов")]
        public string? Description { get; set; }

        [Display(Name = "Код доступа")]
        [Required(ErrorMessage = "Поле \"Код доступа\" пустое")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Код доступа должен содержать от 4 до 8 символов")]
        public string? Code { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Тип группы")]
        [Required(ErrorMessage = "Поле \"Тип группы\" пустое")]
        [ForeignKey(nameof(GroupTypeId))]
        public int? GroupTypeId { get; set; }

        [Display(Name = "Принадлежит")]
        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        public GroupType? GroupType { get; set; }

        public User? User { get; set; }

        [InverseProperty(nameof(Group))]
        public List<GroupUser>? GroupUser { get; } = new List<GroupUser>();

        [InverseProperty(nameof(Group))]
        public List<GroupLearn>? GroupLearn { get; } = new List<GroupLearn>();
    }
}
