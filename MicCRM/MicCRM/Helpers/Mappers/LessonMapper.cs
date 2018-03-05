using MicCRM.Data.Entities;
using MicCRM.Models.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    static class LessonMapper
    {
        public static LessonInfoViewModel Mapping(Lesson lesson)
        {
            return new LessonInfoViewModel()
            {
                Id = lesson.Id,
                StartingDate = lesson.StartingDate.Date,
                EndingDate = lesson.EndingDate.Date,
                Teacher = TeacherMapper.Mapping(lesson.Teacher),
                Technology = TechnologyMapper.Mapping(lesson.Technology)
            };
        }
    }
}
