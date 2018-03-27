using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers.Mappers;
using MicCRM.Models.TechnologyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicCRM.Controllers
{
    [Authorize]
    public class TechnologyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TechnologyController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AllTechnologies()
        {
            var technologies = _dbContext.Technologies;

            List<TechnologyInfoViewModel> model = new List<TechnologyInfoViewModel>();
            foreach (var item in technologies)
            {
                model.Add(TechnologyMapper.Mapping(item));
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Add()
        {
            var technology = _dbContext.Technologies;

            AddEditTechnologyViewModel model = new AddEditTechnologyViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddEditTechnologyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Technology technology = new Technology()
                {
                    Name = model.Name
                };

                _dbContext.Technologies.Add(technology);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(AllTechnologies));
            }
            return View();
        }
    }
}