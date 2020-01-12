using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.EntityFramework;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("joboffer")]
    public class JobOfferController : Controller
    {
        public DataContext context;
        public JobOfferController(DataContext dataContext)
        {
            context = dataContext;
        }
        /// <summary>
        /// Opens a page with index of all job offers
        /// </summary>
        /// <param name="search">search text to be parsed in the job title</param>
        /// <returns>view with the job offers</returns>
        [Route("index")]
        [HttpGet]
        public IActionResult Index(string search)
        {
            if (search == "" || search == null)
                return View(context.JobOffers.Include(o => o.Company).ToList());
            else
                return View(context.JobOffers.Include(o => o.Company).Where(o => o.JobTitle.ToLower().Contains(search.ToLower())).ToList());
        }
        /// <summary>
        /// Opens a page with form to add a job offer
        /// </summary>
        /// <returns>view with the form</returns>
        [Route("Add")]
        public IActionResult AddJobOffer()
        {
            var companies = context.Companies.ToList();
            return View(companies);
        }
        /// <summary>
        /// Saves the job offer from the form
        /// </summary>
        /// <param name="offer">job offer to be saved, taken from the form</param>
        /// <returns>view the index of the job offers or back to the view to correct the data</returns>
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
            return View("AddJobOffer", context.Companies.ToList());
        }
        /// <summary>
        /// Shows a view with details of a job offer
        /// </summary>
        /// <param name="id">id of the job offer, for which details should be shown</param>
        /// <returns>view with the details</returns>
        [Route("Details")]
        public IActionResult Details(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }

        /// <summary>
        /// Deletes a job offer
        /// </summary>
        /// <param name="id">id of the job offer, which should be deleted</param>
        /// <returns>view to the index of job offers</returns>
        [Route("Delete")]
        public IActionResult DeleteOffer(int id)
        {
            context.JobApplications.RemoveRange(context.JobApplications.Where(a => a.OfferId == id));
            context.JobOffers.RemoveRange(context.JobOffers.Where(o => o.Id == id));
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Shows a view with a form to edit a job offer
        /// </summary>
        /// <param name="id">id of the job offer, which should be changed</param>
        /// <returns>view with the edit page</returns>
        [HttpGet]
        [Route("Edit")]
        public IActionResult EditJobOffer(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == id);
            ViewBag.companies = context.Companies.ToList();
            return View(offer);
        }
        /// <summary>
        /// Validates and saves the changed job offer
        /// </summary>
        /// <param name="offer">job offer that was changed</param>
        /// <returns>view with the index or view to edit the incorrect data</returns>
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