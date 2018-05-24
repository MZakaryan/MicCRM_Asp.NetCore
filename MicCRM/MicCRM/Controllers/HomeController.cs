using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MicCRM.Models;
using Microsoft.AspNetCore.Authorization;
using MicCRM.Models.NotificationViewModels;
using MicCRM.Data;
using MicCRM.Helpers.Mappers;
using MicCRM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using MicCRM.Models.ApplicantViewModels;
using MicCRM.Helpers;

namespace MicCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JsonResult GetNotification(int id)
        {
            var not = _dbContext.Notifications
                                .Where(n => n.Id == id)
                                .OrderByDescending(n => n.Date)
                                .Select(n => new
                                {
                                    n.Technology,
                                    n.Phone2,
                                    n.Email
                                });

            return Json(not);
        }

        public IActionResult Index()
        {
            var not = _dbContext.Notifications
                .Where(n => n.IsMuted == false)
                .OrderByDescending(a => a.Date);

            List<NotificationInfoViewModel> notIVM = new List<NotificationInfoViewModel>();
            foreach (var item in not)
            {
                notIVM.Add(NotificationMapper.Mapping(item));
            }

            NotificationViewModel model = new NotificationViewModel()
            {
                Notifications = notIVM,
                Date = DateTime.Now
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var not = _dbContext.Notifications
                                .Find(id);

            var nots = _dbContext.Notifications
                .Where(n => n.IsMuted == false)
                .OrderBy(a => a.Date);

            List<NotificationInfoViewModel> notIVM = new List<NotificationInfoViewModel>();
            foreach (var item in nots)
            {
                notIVM.Add(NotificationMapper.Mapping(item));
            }

            NotificationViewModel model = new NotificationViewModel()
            {
                Notifications = notIVM,
                Id = not.Id,
                FirstName = not.FirstName,
                LastName = not.LastName,
                Email = not.Email,
                Phone1 = not.Phone1,
                Phone2 = not.Phone2,
                Date = not.Date,
                Technology = not.Technology
            };
            
            return View("Index", model);
        }

        public IActionResult MakeApplicant(int id)
        {

            var not = _dbContext.Notifications.Find(id);
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            AddEditApplicantViewModel model = new AddEditApplicantViewModel()
            {
                Id = 0,
                FirstName = not.FirstName,
                LastName = not.LastName,
                Email = not.Email,
                Date = not.Date,
                Phone1 = not.Phone1,
                Phone2 = not.Phone2,
                Lessons = Utilities.GetSelectListItem(lessons)
            };
            return View("../Applicant/Add", model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult AddNotification(NotificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Notification notification = new Notification()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    Email = model.Email,
                    Technology = model.Technology,
                    Date = model.Date
                };
                _dbContext.Entry(notification).State = notification.Id == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public void MuteNotification(params int[] arrayOfId)
        {
            if (arrayOfId.Length == 0)
                return;

            foreach (int id in arrayOfId)
            {
                Notification not = _dbContext.Notifications
                            .Where(a => a.Id == id)
                            .SingleOrDefault();
                not.IsMuted = true;
                _dbContext.Entry(not).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }
    }
}
