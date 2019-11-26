using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.EntityFramework;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationApiController : ControllerBase
    {
        public DataContext context;
        public ApplicationApiController(DataContext dataContext)
        {
            context = dataContext;
        }
        //ajax offer details
        [HttpPost("{id}")]
        public IEnumerable<JobApplication> GetJobApplications(int id)
        {
            var applications = context.JobApplications.Where(a => a.OfferId == id).ToList();
            return applications;
        }
    }
}