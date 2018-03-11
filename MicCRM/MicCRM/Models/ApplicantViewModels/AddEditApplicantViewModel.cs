using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.ApplicantViewModels
{
    public class AddEditApplicantViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> Lesssons { get; set; }

        public int LessonId { get; set; }
    }
}
