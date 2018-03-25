using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.ApplicantViewModels;
using MicCRM.Models.StudentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicantController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult AllApplicants()
        {
            return View(GetAllApplicantIVM());
        }

        public IActionResult GetApplicantsForPartial()
        {
            return View("_ApplicantTablePartial", GetAllApplicantIVM());
        }

        [NonAction]
        private List<ApplicantInfoViewModel> GetAllApplicantIVM()
        {
            var applicants = _dbContext.Applicants
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Teacher)
                            .Include(a => a.Lesson)
                                .ThenInclude(l => l.Technology)
                            .Where(a => a.IsStudent == false)
                            .OrderByDescending(a => a.Date);

            List<ApplicantInfoViewModel> model = new List<ApplicantInfoViewModel>();
            foreach (var item in applicants)
            {
                model.Add(ApplicantMapper.Mapping(item));
            }

            return model;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            AddEditApplicantViewModel model = new AddEditApplicantViewModel()
            {
                Lesssons = GetSelectListItem(lessons)
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
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Lesson = _dbContext.Lessons.Find(model.LessonId),
                };
                _dbContext.Applicants.Add(applicant);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(AllApplicants));
            }
            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetSelectListItem(
            IEnumerable<Lesson> lessons)
        {
            List<SelectListItem> lessonsToSelect = new List<SelectListItem>();

            foreach (Lesson item in lessons)
            {
                lessonsToSelect.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.ToString()
                });
            }

            return lessonsToSelect;
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