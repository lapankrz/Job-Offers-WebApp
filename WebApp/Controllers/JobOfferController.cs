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
        /*
        [Route("index")]
        public async Task<ActionResult> Create()
        {
            var offers = context.JobOffers.ToList();
            return View(offers);
        }
        */
        [Route("index")]
        public IActionResult Index()
        {
            return View(context.JobOffers.ToList());
        }
        

        public IActionResult Details(int id)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
    }
}