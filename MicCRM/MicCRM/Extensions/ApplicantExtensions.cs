using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.ApplicantViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Extensions
{
    public static class ApplicantExtensions
    {
        public static IQueryable<Applicant> Filter(this IQueryable<Applicant> applicants,
            ApplicantSerachViewModel serachModel)
        {
            if (!string.IsNullOrEmpty(serachModel.FirstName))
                applicants = applicants
                    .Where(a => a.FirstName.Contains(serachModel.FirstName));
            if (!string.IsNullOrEmpty(serachModel.LastName))
                applicants = applicants
                    .Where(a => a.LastName.Contains(serachModel.LastName));
            if (serachModel.LessonId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.Id == serachModel.LessonId);
            if (serachModel.TeacherId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.TeacherId == serachModel.TeacherId);
            if (serachModel.TechnologyId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.TechnologyId == serachModel.TechnologyId);

            return applicants;
        }

        public static List<ApplicantInfoViewModel> GetAllApplicantIVM(
            this IQueryable<Applicant> applicants)
        {
            List<ApplicantInfoViewModel> model = new List<ApplicantInfoViewModel>();
            foreach (var item in applicants)
            {
                model.Add(ApplicantMapper.Mapping(item));
            }
            return model;
        }

        public static IQueryable<Applicant> GetApplicants(this ApplicationDbContext dbContext)
        {
            var applicants = dbContext.Applicants
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Teacher)
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Technology)
                            .Where(a => a.IsStudent == false && a.Deleted == false)
                            .OrderByDescending(a => a.Date)
                            .Select(a => a);

            return applicants;
        }

    }
}
