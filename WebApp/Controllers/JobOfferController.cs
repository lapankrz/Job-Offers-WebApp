using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.EntityFramework;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("joboffer")]
    [ApiController]
    public class JobOfferController : Controller
    {
        public DataContext context;
        public JobOfferController(DataContext dataContext)
        {
            context = dataContext;
        }
        [Route("index")]
        [HttpGet]
        public IActionResult Index(string search)
        {
            if (search == "" || search == null)
                return View(context.JobOffers.Include(o => o.Company).ToList());
            else
                return View(context.JobOffers.Include(o => o.Company).Where(o => o.JobTitle.ToLower().Contains(search.ToLower())).ToList());
        }
        
        [Route("Add")]
        public IActionResult AddJobOffer()
        {
            //pobrać firmy do drop down menu
            var companies = context.Companies.ToList();
            return View(companies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveJobOffer(JobOffer offer)
        {
            if (ModelState.IsValid)
            {
                var company = context.Companies.FirstOrDefault(c => c.Id == offer.Company.Id);
                offer.Company = company;
                offer.CreationDate = DateTime.Now;
                context.JobOffers.Add(offer);
                context.SaveChanges();
                return View("/Views/JobOffer/Index.cshtml", context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).ToList());
            }
            return View("AddJobOffer");
        }
        [Route("Details")]
        public IActionResult Details(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
        [Route("Delete")]
        public IActionResult DeleteOffer(int id)
        {
            context.JobApplications.RemoveRange(context.JobApplications.Where(a => a.OfferId == id));
            context.JobOffers.RemoveRange(context.JobOffers.Where(o => o.Id == id));
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Edit")]
        public IActionResult EditJobOffer(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == id);
            ViewBag.companies = context.Companies.ToList();
            return View(offer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public IActionResult EditSaveJobOffer(JobOffer offer)
        {
            if (ModelState.IsValid)
            {
                var company = context.Companies.FirstOrDefault(c => c.Id == offer.Company.Id);
                offer.Company = company;
                context.JobOffers.Update(offer);
                context.SaveChanges();
                return View("/Views/JobOffer/Index.cshtml", context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).ToList());
            }
            return View("EditJobOffer");
        }
    }
}