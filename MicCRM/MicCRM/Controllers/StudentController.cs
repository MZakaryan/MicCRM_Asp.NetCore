using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.StudentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
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