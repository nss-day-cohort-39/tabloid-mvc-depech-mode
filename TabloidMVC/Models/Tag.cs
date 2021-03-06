﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name for your tag...")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "Your tag's name cannot contain any commas!")]
        public string Name { get; set; }
    }
}
