﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        [Key] // Prob not needed since id is already assumed to be primary key
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
