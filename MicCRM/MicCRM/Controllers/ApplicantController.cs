using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.ApplicantViewModels;
using MicCRM.Models.StudentViewModels;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}