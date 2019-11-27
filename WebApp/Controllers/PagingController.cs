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
    [Route("api/[controller]")]
    [ApiController]
    public class PagingController : ControllerBase
    {
        private readonly DataContext context;
        public PagingController(DataContext dataContext)
        {
            context = dataContext;
        }
        [HttpGet]
        public IEnumerable<JobOffer> SearchJobOffers(string searchText)
        {
            var result = context.JobOffers.Include(o => o.Company).Where(o => o.JobTitle.ToLower().Contains(searchText)).ToList();
            return result;
        }
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