﻿using LearnEF.Entities.Base;
using LearnEF.Entities.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
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
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Работа сдана?")]
        public bool IsAttached { get; set; }

        [ForeignKey(nameof(UserId))]
        public string? UserId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public int? GroupId { get; set; }

        public User? User { get; set; }

        [InverseProperty(nameof(Learn))]
        public List<LearnDocuments>? LearnDocuments { get; } = new List<LearnDocuments>();
    }
}
