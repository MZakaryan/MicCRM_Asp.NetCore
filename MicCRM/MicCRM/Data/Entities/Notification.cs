﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Data.Entities
{
    public class Notification : EntityBase
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string  LastName { get; set; }
        [Required]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Technology { get; set; }
        [Required]
        public bool IsMuted { get; set; }
    }
}
