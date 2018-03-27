using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.ApplicantViewModels;
using MicCRM.Models.StudentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicantController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Search(string firstName, string lastName, 
            int lessonId, int teacherId, int technologyId, int? page)
        {
            var applicants = _dbContext.Applicants
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Teacher)
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Technology)
                            .Where(a => a.IsStudent == false && a.Deleted == false)
                            .OrderByDescending(a => a.Date)
                            .Select(a => a);

            if (!string.IsNullOrEmpty(firstName))
                applicants = applicants
                    .Where(a => a.FirstName.Contains(firstName));
            if (!string.IsNullOrEmpty(lastName))
                applicants = applicants
                    .Where(a => a.LastName.Contains(lastName));
            if (lessonId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.Id == lessonId);
            if (teacherId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.TeacherId == teacherId);
            if (technologyId != 0)
                applicants = applicants
                    .Where(a => a.Lesson.TechnologyId == technologyId);

            List<ApplicantInfoViewModel> applicantsIVM = new List<ApplicantInfoViewModel>();
            foreach (var item in applicants)
            {
                applicantsIVM.Add(ApplicantMapper.Mapping(item));
            }

            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            var teachers = _dbContext.Teachers;
            var technologies = _dbContext.Technologies;
            
            int pageSize = 3;
            var paginatedApplicants = PaginatedList<ApplicantInfoViewModel>.Create(applicantsIVM, page ?? 1, pageSize);

            ApplicantsAndLessonsViewModel model = new ApplicantsAndLessonsViewModel()
            {
                PaginatedApplicants = paginatedApplicants,
                Lessons = GetSelectListItem(lessons),
                Teachers = GetSelectListItem(teachers),
                Technologies = GetSelectListItem(technologies),
                FirstName = firstName,
                LastName = lastName,
                LessonId = lessonId,
                TeacherId = teacherId,
                TechnologyId = technologyId
            };

            return View("AllApplicants", model);
        }

        public IActionResult AllApplicants(int? page)
        {
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            var teachers = _dbContext.Teachers;
            var technologies = _dbContext.Technologies;

            var applicants = GetAllApplicantIVM();
            
            int pageSize = 3;
            var paginatedApplicants = PaginatedList<ApplicantInfoViewModel>.Create(applicants, page ?? 1, pageSize);

            ApplicantsAndLessonsViewModel model = new ApplicantsAndLessonsViewModel()
            {
                PaginatedApplicants = paginatedApplicants,
                Lessons = GetSelectListItem(lessons),
                Teachers = GetSelectListItem(teachers),
                Technologies = GetSelectListItem(technologies)
            };
            
            return View(model);
        }

        public IActionResult GetApplicantsForPartial()
        {
            return View("_ApplicantTablePartial", GetAllApplicantIVM());
        }

        [NonAction]
        private IEnumerable<ApplicantInfoViewModel> GetAllApplicantIVM()
        {
            var applicants = _dbContext.Applicants
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Teacher)
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Technology)
                            .Where(a => a.IsStudent == false && a.Deleted == false)
                            .OrderByDescending(a => a.Date);

            List<ApplicantInfoViewModel> model = new List<ApplicantInfoViewModel>();
            foreach (var item in applicants)
            {
                model.Add(ApplicantMapper.Mapping(item));
            }

            return model;
        }

        public IActionResult Edit(int id)
        {
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);
            var applicant = _dbContext.Applicants
                            .Include(a => a.Lesson)
                            .Where(a => a.IsStudent == false && a.Deleted == false)
                            .Where(a => a.Id == id)
                            .Single();

            AddEditApplicantViewModel model = new AddEditApplicantViewModel()
            {
                Id = id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                Phone1 = applicant.Phone1,
                Phone2 = applicant.Phone2,
                Date = applicant.Date,
                LessonId = applicant.Lesson.Id,
                Description = applicant.Description,
                Lessons = GetSelectListItem(lessons)
            };
            return View("Add", model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            AddEditApplicantViewModel model = new AddEditApplicantViewModel()
            {
                Lessons = GetSelectListItem(lessons)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddEditApplicantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Applicant applicant = new Applicant()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Lesson = _dbContext.Lessons.Find(model.LessonId),
                };
                _dbContext.Entry(applicant).State = applicant.Id == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(AllApplicants));
            }
            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetSelectListItem<TEntity>(IEnumerable<TEntity> tEntity)
            where TEntity : EntityBase
        {
            List<SelectListItem> tEntityToSelect = new List<SelectListItem>();

            foreach (TEntity item in tEntity)
            {
                tEntityToSelect.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.ToString()
                });
            }

            return tEntityToSelect;
        }

        [HttpPost]
        public void Delete(params int[] arrayOfId)
        {
            if (arrayOfId.Length == 0)
                return;

            foreach (int id in arrayOfId)
            {
                Applicant applicant = _dbContext.Applicants
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
                applicant.Deleted = true;
                _dbContext.Entry(applicant).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }

        [HttpPost]
        public void MakeStudent(params int[] arrayOfId)
        {
            if (arrayOfId.Length == 0)
                return;

            foreach (int id in arrayOfId)
            {
                Applicant applicant = _dbContext.Applicants
                            .Include(a => a.Lesson)
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
                applicant.IsStudent = true;
                AddStudent(applicant);
                _dbContext.Entry(applicant).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }

        [NonAction]
        private void AddStudent(Applicant applicant)
        {
            Student student = new Student()
            {
                ApplicantId = applicant.Id,
                Description = applicant.Description
            };

            StudentLessons studentLessons = new StudentLessons()
            {
                Student = student,
                Lesson = _dbContext.Lessons.Find(applicant.Lesson.Id),
            };

            _dbContext.Add(studentLessons);
        }

        public JsonResult GetLesson(int id)
        {
            var lesson = from l in _dbContext.Lessons
                                        .Include(l => l.Teacher)
                                        .Include(l => l.Technology)
                                        .Where(l => l.Id == id)
                         select new
                         {
                             l.Technology.Name,
                             l.Teacher.FirstName,
                             l.Teacher.LastName,
                             l.StartingDate,
                             l.EndingDate
                         };

            return Json(lesson);
        }

    }
}