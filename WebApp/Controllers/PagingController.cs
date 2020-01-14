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
    /// <summary>
    /// controller for paging
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PagingController : ControllerBase
    {
        private readonly DataContext context;
        /// <summary>
        /// constructor for the controller
        /// </summary>
        /// <param name="dataContext">context for the database</param>
        public PagingController(DataContext dataContext)
        {
            context = dataContext;
        }
        /// <summary>
        /// Searches the job offers with job titles containing the search text
        /// </summary>
        /// <param name="searchText">text to be searched for in job titles</param>
        /// <returns>list of job offers with the search text in the job title</returns>
        [HttpGet]
        public IEnumerable<JobOffer> SearchJobOffers(string searchText)
        {
            var result = context.JobOffers.Include(o => o.Company).Where(o => o.JobTitle.ToLower().Contains(searchText)).ToList();
            return result;
        }
        /// <summary>
        /// Gets one page of job offers
        /// </summary>
        /// <param name="pageNo">the number of the page to be returned</param>
        /// <param name="pageSize">size of the page in number of job offers</param>
        /// <returns>page of job offers</returns>
        [HttpPost("{pageNo}")]
        public PagingViewModel GetJobOffers(int pageNo, int pageSize = 6)
        {
            int totalOffers = context.JobOffers.Count();
            int totalPage = (totalOffers / pageSize) + ((totalOffers % pageSize) > 0 ? 1 : 0);
            var offers = context.JobOffers.Include(o => o.Company).OrderBy(o => o.JobTitle).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            var model = new PagingViewModel
            {
                Offers = offers,
                TotalPage = totalPage
            };
            return model;
        }
}
}