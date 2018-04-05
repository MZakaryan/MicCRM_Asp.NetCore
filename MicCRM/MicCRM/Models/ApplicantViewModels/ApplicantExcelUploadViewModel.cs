using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.ApplicantViewModels
{
    public class ApplicantExcelUploadViewModel
    {
        [Required]
        public IFormFile ExcelFile { get; set; }
        public IEnumerable<SelectListItem> Lessons { get; set; }
        public int LessonId { get; set; }
    }
}
