using MicCRM.Data.Entities;
using MicCRM.Models.ApplicanViewModels;
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
    }
}
