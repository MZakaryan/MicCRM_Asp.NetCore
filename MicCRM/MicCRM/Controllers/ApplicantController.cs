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
            var applicants = _dbContext.Applicants
                .Include(a => a.Lesson)
                    .ThenInclude(l => l.Teacher)
                .Include(a => a.Lesson)
                    .ThenInclude(l => l.Technology);

            List<ApplicantInfoViewModel> model = new List<ApplicantInfoViewModel>();
            foreach (var item in applicants)
            {
                model.Add(ApplicantMapper.Mapping(item));
            }
            return View(model);
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

        public JsonResult GetLesson(int id)
        {
            var lesson = from l in _dbContext.Lessons where l.Id == id
                         join teacher in _dbContext.Teachers on l.TeacherId equals teacher.Id
                         join tech in _dbContext.Technologies on l.TechnologyId equals tech.Id
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