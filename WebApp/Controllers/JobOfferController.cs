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
    [Route("joboffer")]
    public class JobOfferController : Controller
    {
        public DataContext context;
        public JobOfferController(DataContext dataContext)
        {
            context = dataContext;
        }
        [Route("index")]
        public IActionResult Index()
        {
            return View(context.JobOffers.ToList());
        }
        [Route("AddJobOffer")]
        public IActionResult AddJobOffer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveJobOffer(JobOffer offer)
        {
            if (ModelState.IsValid)
            {
                var company = context.Companies.FirstOrDefault(c => c.Id == offer.Id);
                offer.Company = company;
                offer.Id = 0;
                offer.CreationDate = DateTime.Now;
                context.JobOffers.Add(offer);
                context.SaveChanges();
                return View("/Views/JobOffer/Index.cshtml", context.JobOffers.ToList());
            }
            return View("AddJobOffer");
        }
        [Route("Details")]
        public IActionResult Details(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
        [Route("DeleteOffer")]
        public IActionResult DeleteOffer(int id)
        {
            context.JobApplications.RemoveRange(context.JobApplications.Where(a => a.OfferId == id));
            context.JobOffers.RemoveRange(context.JobOffers.Where(o => o.Id == id));
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}