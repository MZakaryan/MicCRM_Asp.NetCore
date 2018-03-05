using MicCRM.Models.TeacherViewModels;
using MicCRM.Models.TechnologyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Models.LessonViewModels
{
    public class LessonInfoViewModel
    {
        public int Id { get; set; } 
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public TeacherInfoViewModel Teacher { get; set; }
        public TechnologyInfoViewModel Technology { get; set; }

        public override string ToString()
        {
            return $"{StartingDate.ToShortDateString()} {Technology.Name}";
        }

    }
}
