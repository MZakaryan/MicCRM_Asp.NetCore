using MicCRM.Models.LessonViewModels;
using System;

namespace MicCRM.Models.ApplicantViewModels
{
    public class ApplicantInfoViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public LessonInfoViewModel Lesson { get; set; }
        public bool IsStudent { get; set; }
    }
}
