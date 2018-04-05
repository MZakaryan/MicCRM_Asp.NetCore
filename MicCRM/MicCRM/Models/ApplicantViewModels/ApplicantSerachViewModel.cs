using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.ApplicantViewModels
{
    public class ApplicantSerachViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LessonId { get; set; }
        public int TeacherId { get; set; }
        public int TechnologyId { get; set; }
        public int? PageIndex { get; set; }
    }
}
