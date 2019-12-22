using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.EntityFramework;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("apply")]
    public class JobApplicationController : Controller
    {
        private readonly DataContext context;
        public JobApplicationController(DataContext dataContext)
        {
            context = dataContext;
        }
        [HttpGet]
        public IActionResult Form(int id)
        {
            JobOffer offer = context.JobOffers.Include(o => o.Company).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(JobApplication application)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == application.OfferId);
            if (ModelState.IsValid)
            {
                offer.JobApplications.Add(application);
                context.JobApplications.Add(application);
                context.SaveChanges();
                return View("/Views/JobOffer/Details.cshtml", offer);
            }
            return View("/Views/JobApplication/Form.cshtml", offer);
        }
    }
}