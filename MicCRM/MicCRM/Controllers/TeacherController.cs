using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.TeacherViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicCRM.Controllers
{
    //[Authorize]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TeacherController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AllTeachers()
        {
            var teachers = _dbContext.Teachers;

            List<TeacherInfoViewModel> model = new List<TeacherInfoViewModel>();
            foreach (var item in teachers)
            {
                model.Add(TeacherMapper.Mapping(item));
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Add()
        {
            var teachers = _dbContext.Teachers;

            AddEditTeacherViewModel model = new AddEditTeacherViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddEditTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone1,
                    Email = model.Email
                };
                _dbContext.Teachers.Add(teacher);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(AllTeachers));
            }
            return View();
        }
    }
}