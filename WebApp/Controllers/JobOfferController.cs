using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("joboffer")]
    public class JobOfferController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View(_jobOffers);
        }

        public IActionResult Details(int id)
        {
            var offer = _jobOffers.FirstOrDefault(o => o.Id == id);
            return View(offer);
        }

        private static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer{Id = 1, JobTitle = "Backend Developer" },
            new JobOffer{Id = 2, JobTitle = "Frontend Developer"},
            new JobOffer{Id = 3, JobTitle = "Manager"},
            new JobOffer{Id = 4, JobTitle = "Teacher"},
            new JobOffer{Id = 5, JobTitle = "Cook"}
        };
    }
}