using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.StudentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
    //[Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _dbContex;

        public StudentController(ApplicationDbContext dbContext)
        {
            _dbContex = dbContext;
        }

        public IActionResult AllStudents()
        {
            var students = _dbContex.Students
                .Include(s => s.Applicant)
                .Include(s => s.StudentLessons)
                    .ThenInclude(sl => sl.Lesson)
                        .ThenInclude(l => l.Teacher)
                .Include(s => s.StudentLessons)
                    .ThenInclude(sl => sl.Lesson)
                        .ThenInclude(l => l.Technology);

            List<StudentInfoViewModel> model = new List<StudentInfoViewModel>();
            foreach (var item in students)
            {
                model.Add(StudentMapper.Mapping(item));
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddLessonToStudent(int lessonId, params int[] arrayOfStudentId)
        {
            if (arrayOfStudentId.Length == 0)
                return Json(false);

            var lesson = _dbContex.Lessons
                         .Where(l => l.Id == lessonId)
                         .SingleOrDefault();
            foreach (int id in arrayOfStudentId)
            {
                var student = _dbContex.Students
                              .Where(s => s.Id == id)
                              .SingleOrDefault();
                StudentLessons studentLessons = new StudentLessons()
                {
                    Lesson = lesson,
                    Student = student
                };
                _dbContex.Add(studentLessons);
            }
            bool flag = true ? _dbContex.SaveChanges() != 0 : false;
            return Json(flag);
        }

        public JsonResult GetLessons(int id)
        {
            var les = from l in _dbContex.Students
                      .Include(s => s.StudentLessons)
                          .ThenInclude(sl => sl.Lesson)
                              .ThenInclude(l => l.Teacher)
                      .Include(s => s.StudentLessons)
                          .ThenInclude(sl => sl.Lesson)
                              .ThenInclude(l => l.Technology)
                      .Where(s => s.Id == id)
                      .FirstOrDefault()
                      .StudentLessons
                      .Select(sl => sl.Lesson)
                      select new
                      {
                          l.Technology.Name,
                          l.Teacher.FirstName,
                          l.Teacher.LastName,
                          l.StartingDate,
                          l.EndingDate
                      };

            return Json(les);
        }
    }
}