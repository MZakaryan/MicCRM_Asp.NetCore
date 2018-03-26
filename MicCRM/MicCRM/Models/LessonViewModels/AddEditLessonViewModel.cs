using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.LessonViewModels
{
    public class AddEditLessonViewModel
    {
        public int? Id { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        [Required]
        public DateTime EndingDate { get; set; }

        public IEnumerable<SelectListItem> Technologies { get; set; }
        public IEnumerable<SelectListItem> Teachers { get; set; }

        public int TechnologyId { get; set; }
        public int TeacherId { get; set; }
    }
}
