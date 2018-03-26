using MicCRM.Models.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Models.StudentViewModels
{
    public class StudentInfoViewModel
    {
        public StudentInfoViewModel()
        {
            Lessons = new List<LessonInfoViewModel>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool IsWorker { get; set; }
        public List<LessonInfoViewModel> Lessons { get; set; }
    }
}
