using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicCRM.Data.Entities
{
    public class Lesson : EntityBase
    {
        public Lesson()
        {
            LessonStudents = new List<StudentLessons>();
        }

        [Required]
        public DateTime StartingDate { get; set; }

        [Required]
        public DateTime EndingDate { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }


        [ForeignKey(nameof(Technology))]
        public int TechnologyId { get; set; }
        public virtual Technology Technology { get; set; }

        public virtual ICollection<StudentLessons> LessonStudents { set; get; }
        
        public override string ToString()
        {
            return $"{Technology.Name}, {Teacher.FirstName} {Teacher.LastName}, {StartingDate.ToShortDateString()} - {EndingDate.ToShortDateString()}";
        }
    }
}