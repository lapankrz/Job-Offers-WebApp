using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Route("apply")]
    public class JobApplicationController : Controller
    {
        [HttpGet]
        public IActionResult Form(int id)
        {
            JobApplication application = new JobApplication();
            application.OfferId = id;
            return View(application);
        }
        [HttpPost]
        public IActionResult SaveForm(JobApplication application)
        {
            JobOffer offer = JobOfferController._jobOffers.FirstOrDefault(o => o.Id == application.OfferId);
            offer.JobApplications.Add(application);
            return View("/Views/JobOffer/Details.cshtml", offer);
        }
    }
}