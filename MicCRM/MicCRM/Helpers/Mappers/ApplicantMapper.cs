using MicCRM.Data.Entities;
using MicCRM.Models.ApplicantViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    static class ApplicantMapper
    {
        public static ApplicantInfoViewModel Mapping(Applicant applicant)
        {
            return new ApplicantInfoViewModel()
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Phone1 = applicant.Phone1,
                Phone2 = applicant.Phone2,
                Email = applicant.Email,
                Description = applicant.Description,
                Date = applicant.Date.Date,
                Lesson = LessonMapper.Mapping(applicant.Lesson)
            };
        }

        public static ApplicantsAndLessonsViewModel Mapping(
            PaginatedList<ApplicantInfoViewModel> paginatedApplicants,
            IEnumerable<Lesson> lessons, IEnumerable<Teacher> teachers,
            IEnumerable<Technology> technologies,
            string firstName, string lastName,int lessonId, int teacherId, int technologyId,
            int page)
        {
            ApplicantsAndLessonsViewModel model = new ApplicantsAndLessonsViewModel()
            {
                PaginatedApplicants = paginatedApplicants,
                Lessons = Utilities.GetSelectListItem(lessons),
                Teachers = Utilities.GetSelectListItem(teachers),
                Technologies = Utilities.GetSelectListItem(technologies),
                FirstName = firstName,
                LastName = lastName,
                LessonId = lessonId,
                TeacherId = teacherId,
                TechnologyId = technologyId,
                PageIndex = page
            };

            return model;
        }

        public static ApplicantSerachViewModel Mapping(
            string firstName, string lastName, 
            int lessonId, int teacherId, int technologyId)
        {
            return new ApplicantSerachViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                LessonId = lessonId,
                TeacherId = teacherId,
                TechnologyId = technologyId
            };
        }
    }
}
