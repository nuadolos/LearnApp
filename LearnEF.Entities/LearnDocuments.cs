﻿using LearnEF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Entities
{
    [Table("LearnDocuments")]
    public class LearnDocuments : EntityBase
    {
        [Display(Name = "Материал")]
        [ForeignKey(nameof(LearnId))]
        public int? LearnId { get; set; }

        [Display(Name = "Документ")]
        public byte[]? FileDoc { get; set; }

        public Learn? Learn { get; set; }
    }
}