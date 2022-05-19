﻿using LearnEF.Entities.Base;
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
    [Table("Friend")]
    public class Friend : EntityBase
    {
        [Display(Name = "Отправил заявку")]
        [Required]
        [ForeignKey(nameof(SentUserId))]
        public string? SentUserId { get; set; }

        [Display(Name = "Принял заявку")]
        [Required]
        [ForeignKey(nameof(AcceptedUserId))]
        public string? AcceptedUserId { get; set; }

        [Display(Name = "Подружились")]
        [Required]
        [Column(TypeName = "date")]
        public DateTime? MakeFriend { get; set; }

        public User? SentUser { get; set; }
        public User? AcceptedUser { get; set; }
    }
}
