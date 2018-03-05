using MicCRM.Data.Entities;
using MicCRM.Models.LessonViewModels;
using MicCRM.Models.StudentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    static class StudentMapper
    {
        public static StudentInfoViewModel Mapping(Student student)
        {
            List<Lesson> lessonList = new List<Lesson>();
            lessonList = student.StudentLessons.Select(sl => sl.Lesson).ToList();
            List<LessonInfoViewModel> lessonIVMList = new List<LessonInfoViewModel>();

            foreach (Lesson item in lessonList)
            {
                lessonIVMList.Add(LessonMapper.Mapping(item));
            }

            return new StudentInfoViewModel()
            {
                Id = student.Id,
                FirstName = student.Applicant.FirstName,
                LastName = student.Applicant.LastName,
                Phone1 = student.Applicant.Phone1,
                Phone2 = student.Applicant.Phone2,
                Email = student.Applicant.Email,
                Description = student.Description,
                X = student.X,
                Y = student.Y,
                Z = student.Z,
                Date = student.Applicant.Date.Date,
                Lessons = lessonIVMList,
            };
        }
    }
}
