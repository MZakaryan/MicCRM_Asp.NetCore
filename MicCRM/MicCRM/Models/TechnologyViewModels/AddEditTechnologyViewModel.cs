using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.TechnologyViewModels
{
    public class AddEditTechnologyViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
