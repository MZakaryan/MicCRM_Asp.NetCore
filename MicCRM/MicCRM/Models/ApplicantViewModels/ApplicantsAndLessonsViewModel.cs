﻿using MicCRM.Data.Entities;
using MicCRM.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MicCRM.Models.ApplicantViewModels
{
    public class ApplicantsAndLessonsViewModel
    {
        public IEnumerable<SelectListItem> Lessons { get; set; }
        public IEnumerable<SelectListItem> Teachers { get; set; }
        public IEnumerable<SelectListItem> Technologies { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LessonId { get; set; }
        public int TeacherId { get; set; }
        public int TechnologyId { get; set; }
        public int PageIndex { get; set; }
        public PaginatedList<ApplicantInfoViewModel> PaginatedApplicants { get; set; }
    }
}
