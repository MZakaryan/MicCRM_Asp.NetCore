﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.LessonViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public LessonController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AllLessons()
        {
            var lessons = _dbContext.Lessons
                .Include(a => a.Teacher)
                .Include(a => a.Technology);

            List<LessonInfoViewModel> model = new List<LessonInfoViewModel>();
            foreach (var item in lessons)
            {
                model.Add(LessonMapper.Mapping(item));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var teachers = _dbContext.Teachers;
            var technologies = _dbContext.Technologies;

            AddEditLessonViewModel model = new AddEditLessonViewModel()
            {
                Technologies = SelectListItem(technologies),
                Teachers = SelectListItem(teachers)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddEditLessonViewModel model)
        {
            if (ModelState.IsValid)
            {
                Lesson lesson = new Lesson()
                {
                    StartingDate = model.StartingDate,
                    EndingDate = model.EndingDate,
                    Teacher = _dbContext.Teachers.Find(model.TeacherId),
                    Technology = _dbContext.Technologies.Find(model.TechnologyId)
                };
                _dbContext.Lessons.Add(lesson);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(AllLessons));
            }
            return View();
        }
        
        [NonAction]
        private IEnumerable<SelectListItem> SelectListItem<TEntity>(IEnumerable<TEntity> tEntity) 
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
    }
}